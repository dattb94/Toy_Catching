using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TestCampain : MonoBehaviour {
    public Text textIndex, textInput;
    void Start()
    {
        TestStartCampain.LoadDataLevel();
        int i = TestStartCampain._indexer + 1;
        textIndex.text = "Level: " + i;
    }
    public void ButtonSaveClick()
    {
        TestStartCampain.LoadDataLevel();

        if (TestStartCampain._indexer == TestStartCampain._level)
        {
            TestStartCampain._level += 1;
            TestStartCampain._star.Add(System.Int32.Parse(textInput.text));
            TestStartCampain.Save_Level();
            TestStartCampain.Save_Star();
            Application.LoadLevel(Application.loadedLevelName);
        }
        if(TestStartCampain._indexer < TestStartCampain._level)
        {
            if (TestStartCampain._star[TestStartCampain._indexer] < System.Int32.Parse(textInput.text))
            {
                print("2");
                TestStartCampain._star[TestStartCampain._indexer] = System.Int32.Parse(textInput.text);
                TestStartCampain.Save_Star();
                Application.LoadLevel(Application.loadedLevelName);
            }
            else
            {
                Application.LoadLevel(Application.loadedLevelName);
            }
        }
    }
}
