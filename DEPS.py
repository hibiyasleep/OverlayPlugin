# If you're familiar with Chromium or other Google projects:
#   Sorry, this file actually has nothing to do with gclient.
#   I just borrowed the basic concept and naming scheme.

deps = {
    'ACT': {
        'url': 'https://github.com/EQAditu/AdvancedCombatTracker/releases/download/3.4.5.265/ACTv3.zip',
        'dest': 'Thirdparty/ACT',
        'strip': 0,
        'hash': ['sha256', 'adf13a38d0938ce90f8e674f8365b227d933b91636ddf72b26c85702f6e3b808'],
    },
    'FFXIV_ACT_Plugin': {
        'url': 'https://github.com/ravahn/FFXIV_ACT_Plugin/raw/master/Releases/FFXIV_ACT_Plugin_SDK_2.0.4.0.zip',
        'dest': 'Thirdparty/FFXIV_ACT_Plugin',
        'strip': 0,
        'hash': ['sha256', '006a8372dbb4e0f9761a1f29a926c81db0f41df0ededf378912961e23e9d24b3'],
    },
    'curl': {
        'url': 'https://curl.haxx.se/download/curl-7.67.0.tar.xz',
        'dest': 'Thirdparty/curl',
        'strip': 1,
        'hash': ['sha256', '30fad2e7c3f7b3b5a91d7f4e0f15673b4b5fb244c902a16c8cfb163446d37bf7'],
    },
}
