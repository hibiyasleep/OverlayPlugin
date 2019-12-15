import sys
import os.path
import glob
import xml.etree.ElementTree as ET
import json
from xml.parsers import expat

BASE_DIR = os.path.join(os.path.dirname(__file__), '..')
PROJECTS = ('HtmlRenderer', 'OverlayPlugin', 'OverlayPlugin.Common', 'OverlayPlugin.Core', 'OverlayPlugin.Updater')
ATTRIB_WHITELIST = ('Text',)
JSON_PATH = os.path.join(BASE_DIR, 'translations.json')

# This is an overcomplicated parser which only replaces the contents of <value> tags and adds new <data> nodes.
# The document style (comments, indentation, etc.) is left as-is to reduce noise due to Visual Studio or other tools
# modifying the same file.

class XmlUpdater:

    def __init__(self, path, trans):
        self._path = path
        self._trans = trans
        self._key = None
        self._pos = 0
        self._output = []
        self._done = set()

        with open(path, 'rb') as stream:
            self._data = stream.read()

        self._parser = expat.ParserCreate('utf8')
        self._parser.StartElementHandler = self.start_element
        self._parser.EndElementHandler = self.end_element
        self._parser.Parse(self._data.decode('utf8'))

        self._output.append(self._data[self._pos:])

    def dumps(self):
        return b''.join(self._output)

    def escape(self, value):
        return value.replace('&', '&amp;') \
            .replace('<', '&lt;') \
            .replace('>', '&gt;') \
            .replace('\r', '&#xD;') \
            .replace('\n', '\r\n') \
            .encode('utf8')

    def start_element(self, name, attrs):
        if name == 'data':
            self._key = attrs.get('name')
        elif name == 'value':
            parser_pos = self._parser.CurrentByteIndex
            cut = self._data.find(b'>', parser_pos) + 1

            self._output.append(self._data[self._pos:cut])
            self._pos = cut

    def end_element(self, name):
        if name == 'value' and self._key:
            parser_pos = self._parser.CurrentByteIndex
            old_value = self._data[self._pos:parser_pos]

            if self._key in self._trans:
                new_value = self.escape(self._trans[self._key])

                if old_value != new_value:
                    print('Updating %s (%s -> %s)' % (self._key, old_value.decode('utf8'),
                                                      new_value.decode('utf8')))

                self._output.append(self.escape(self._trans[self._key]))
                self._done.add(self._key)

                # Skip input until </value>
                self._pos = parser_pos
        elif name == 'root':
            # Insert all missing elements

            parser_pos = self._parser.CurrentByteIndex
            self._output.append(self._data[self._pos:parser_pos])
            self._pos = parser_pos

            for key in set(self._trans.keys()) - self._done:
                print('Adding ' + key)
                self._output.append(b'  <data name="%s" xml:space="preserve">\n' % self.escape(key))
                self._output.append(b'    <value>%s</value>\n' % self.escape(self._trans[key]))
                self._output.append(b'  </data>\n')
        elif name == 'data':
            self._key = None


print('Parsing .resx files...')
resx_data = {}
for project in PROJECTS:
    for filepath in glob.iglob(os.path.join(BASE_DIR, project, '**/*.resx'), recursive=True):
        relpath = os.path.relpath(filepath, BASE_DIR).replace('\\', '/')
        chunks = os.path.basename(relpath).split('.')
        lang = 'en'

        if len(chunks) == 3:
            lang = chunks[1]
        elif len(chunks) != 2:
            print('Skipped invalid path: %s' % relpath)
            continue

        relpath = os.path.dirname(relpath) + '/' + chunks[0]
        root = ET.parse(filepath)
        file_data = resx_data.setdefault(relpath, {})
        for node in root.iter('data'):
            key = node.attrib['name']

            if '.' in key and not key.endswith(ATTRIB_WHITELIST):
                continue

            if node.attrib.get('type', '') == 'System.Resources.ResXFileRef, System.Windows.Forms':
                continue

            item = file_data.setdefault(key, {})
            item[lang] = node.find('value').text

            if lang == 'en':
                comment = node.find('comment')
                if comment is not None:
                    item['#comment'] = comment.text


print('Parsing .json file...')
json_data = {}
with open(JSON_PATH, 'r', encoding='utf8') as stream:
    json_data = json.load(stream)


if '--resx' in sys.argv:
    for path, items in json_data.items():
        for key, values in items.items():
            resx_data.setdefault(path, {}).setdefault(key, {}).update(values)

    file_bucket = {}
    for path, items in resx_data.items():
        for key, values in items.items():
            for lang, text in values.items():
                if lang == '#comment':
                    continue
                elif lang == 'en':
                    suffix = '.resx'
                else:
                    suffix = '.%s.resx' % lang

                file_bucket.setdefault(path + suffix, {})[key] = text

    for path, items in file_bucket.items():
        path = os.path.join(BASE_DIR, path)

        if not os.path.isfile(path):
            print('ERROR: %s is missing. Skipping.' % path)
            continue

        print('Opening %s...' % path)
        updater = XmlUpdater(path, items)

        with open(path, 'wb') as stream:
            stream.write(updater.dumps())

elif '--json' in sys.argv:
    for path, items in resx_data.items():
        for key, values in items.items():
            json_data.setdefault(path, {}).setdefault(key, {}).update(values)

    with open(JSON_PATH, 'w', encoding='utf8', newline='\n') as stream:
        json.dump(json_data, stream, ensure_ascii=False, indent=4, sort_keys=True)

else:
    print('Usage: sync_translations.py [--resx|--json]')
    print('')
    print('  --resx    Updates the .resx files with new and modified strings from the .json file.')
    print('  --json    Updates the .json file with new and modified strings from the .resx files.')
