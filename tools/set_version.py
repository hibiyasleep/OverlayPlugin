import sys
import os.path
import re

BASE_DIR = os.path.join(os.path.dirname(__file__), '..')
PROJECTS = ('HtmlRenderer', 'OverlayPlugin', 'OverlayPlugin.Common', 'OverlayPlugin.Core', 'OverlayPlugin.Updater')

VER_RE = re.compile(r'^([0-9]+)\.([0-9]+)\.([0-9]+)$')

# [assembly: AssemblyVersion("0.9.0.0")]
ASM_VER_RE = re.compile(r'\[assembly: AssemblyVersion\("([0-9]+\.[0-9]+\.[0-9]+)\.[0-9]+"\)\]')

old_version = None
new_version = None


def replace_helper(m):
    global old_version

    old = m.group(0)
    if old_version is None:
        old_version = m.group(1)
    elif old_version != m.group(1):
        print('Warning: Found version %s in one of the projects that didn\'t match %s from others.' %
              (m.group(1), old_version))

    return old.replace(m.group(1), new_version)


if len(sys.argv) < 2:
    print('Usage: set_version.py <version>')
    sys.exit(0)

new_version = sys.argv[1]

if not VER_RE.fullmatch(new_version):
    print('Invalid version %s passed!' % new_version)
    sys.exit(1)

for project in PROJECTS:
    path = os.path.join(BASE_DIR, project, 'Properties', 'AssemblyInfo.cs')

    with open(path, 'r', encoding='utf8') as stream:
        data = stream.read()

    new_data = ASM_VER_RE.sub(replace_helper, data)
    if data == new_data:
        print('Warning: %s is unchanged!' % path)

    with open(path, 'w', encoding='utf8', newline='\r\n') as stream:
        stream.write(new_data)
