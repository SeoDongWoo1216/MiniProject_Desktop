## [네이버 오픈 API를 연동한 영화 즐겨찾기 시스템](https://github.com/SeoDongWoo1216/MiniProject_Desktop/tree/main/WPFMiniProject/210401_NaverMovieFinderApp)
1. 네이버에서 오픈 Api를 받아서 검색한 영화를 WPF 그리드에 출력합니다.
2. 즐겨찾기 버튼 이벤트
- '즐겨찾기 추가' 버튼을 통해 DB에 데이터를 Insert 합니다. (이때 그리드의 셀을 클릭한 상태여야 Insert 가능)
- '즐겨찾기 보기' 버튼을 통해 DB의 데이터를 Select 합니다. (보기 상태에서 '즐겨찾기추가' 버튼을 작동하지않도록 제어)
- '즐겨찾기 삭제' 버튼을 통해 DB의 데이터를 Delete 합니다. ('즐겨찾기보기'의 데이터를 삭제)
3. '네이버 영화' 버튼을 통해 네이버영화 사이트에서 해당 영화를 검색했을때의 사이트 링크를 띄워줍니다.(하이퍼 링크 사용)
4. '예고편 보기' 버튼을 통해 새로운 창이 열리면서 유튜브에 기재된 예고편 영상을 볼 수 있습니다.(Youtube API와 연동하여 영상 출력) 


![즐겨찾기](https://github.com/SeoDongWoo1216/MiniProject_Desktop/blob/main/WPFMiniProject/210401_NaverMovieFinderApp/result_Image/210401_NaverMovieFinderApp_result_001.gif "즐겨찾기 이벤트(인셉션검색으로 시작)")
![예고편보기](https://github.com/SeoDongWoo1216/MiniProject_Desktop/blob/main/WPFMiniProject/210401_NaverMovieFinderApp/result_Image/210401_NaverMovieFinderApp_result_002.gif "예고편(예고편보기 버튼으로 시작)")

[소스분석](https://github.com/SeoDongWoo1216/MiniProject_Desktop/tree/main/WPFMiniProject/210401_NaverMovieFinderApp)
