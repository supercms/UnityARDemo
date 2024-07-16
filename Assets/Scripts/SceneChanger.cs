using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * @class
 * @classdesc Scene 전환을 위한 클래스
 * UI 이벤트와 연동하기 위함
 */
public class SceneChanger : MonoBehaviour
{
    /*
     * 메인씬 로드, 매인씬 이름은 미리 "Main"으로 변경하여야 한다.
     */
    public void GotoMain()
    {
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }

    /*
     * 입력 이름과 동일한 씬을 로드함.
     */
    public void GotoScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

}
