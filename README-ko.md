# OverlayPlugin

ACT에서 사용할 수 있는 커스터마이징 가능한 타임라인 및 오버레이를 제공합니다.

## 다운로드

릴리즈 페이지에서 사전 배포본과 배포본을 다운로드 받을 수 있습니다. [release page](https://github.com/RainbowMage/OverlayPlugin/releases).

## 시스템 요구사항

* 닷넷 프레임워크 4.5

## 빌드 방법

* 4.5.1 버전 이상의 닷넷프레임워크를 설치합니다.
* [Microsoft Build Tools 2013](http://www.microsoft.com/ja-jp/download/details.aspx?id=40760). 를 설치합니다. (단, Visual Studio 2013 이상이 이미 설치된 경우 필요하지 않습니다.)
* 깃허브에서 내려받거나 소스 압축파일을 내려받아 압축을 해제합니다.
* 실행 가능한 Advanced Combat Tracker 파일 (`Advanced Combat Tracker.exe`)을 `Thirdparty\ACT` 폴더에 복사합니다.
* `build.bat`을 실행합니다.

작업이 끝나면 플러그인 파일 `OverlayPlugin.dll`이 `Build` 폴더에 생성됩니다.

## 사용 방법

이 플러그인을 사용하려면 먼저 `OverlayPlugin.dll` 을 ACT Plugin 탭에 추가해야 합니다. 함께 포함된 파일과 분리해서는 안되며 이 포함된 파일들은 반드시 필요한 파일입니다.

직접 첫 설치를 한 경우 No Data to show 라는 메세지가 적힌 오버레이 창을 볼 수 있으며 여러분은 이 창의 불투명한 부분을 클릭해 드래그하거나 오른쪽 하단의 모서리에 마우스를 올려 윈도우 크기를 조절할 수 있습니다 (좀 어려워 보이긴 하지만).

ACT의 플러그인 탭에서 `OveralyPlugin.dll` 항목에서 여러분은 설정을 변경하거나 파일 주소 또는 웹 주소를 입력할 수 있고, 클릭 통과 같은 옵션을 설정할 수 있습니다.

예제 웹 문서는 `Build\resources`에 포함되어 있습니다.

## 문제 해결

오버레이가 보이지 않으면 여러분은 ACT Plugins 탭에 포함된 `OverlayPlugin.dll` 에서 Overlay Logs 탭을 확인할 수 있습니다.

### `Error: AssemblyResolve: => System.NotSupportedException`

만약 여러분이 기본 윈도우 압축 해제 방식을 사용한다면, 인터넷에서 다운로드 받은 압축 파일을 신뢰하지 못하는 파일로 분류할 것입니다.

이러한 문제가 발생하는 경우 실행 파일이나 DLL 파일이 손상될 수 있으며 이로 인해 오류를 야기할 수 있습니다. 이곳을 참조하여 문제를 확인합니다. http://blogs.msdn.com/b/delay/p/unblockingdownloadedfile.aspx.

윈도우 탐색기에서 DLL파일을 우클릭하여 속성에 들어가 하단의 차단 해제 버튼을 눌러 해결할 수 있습니다.

만약 네트워크 드라이브를 사용하고 있다면 발생할 수 있으며 이는 로컬 드라이브로 복사하여 해결할 수 있습니다.

### `Error: AssemblyResolve: => System.IO.FileNotFoundException`

`OverlayPlugin.dll` 에서 필요한 파일이 없을 경우 발생합니다.

플러그인 폴더에 존재하는 모든 파일을 복사하여 옮겨주어야 정상적으로 동작합니다.

## 라이센스

MIT 라이센스. LICENSE.txt 에 자세한 정보가 적혀 있습니다.