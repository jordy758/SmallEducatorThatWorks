﻿using System.Collections;
using System.Collections.Generic;
using Source.Composite;
using Source.GUI;
using Source.LeafBehaviour;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Video;
using Component = Source.Composite.Component;

namespace Source
{
    public class SmallEducator : MonoBehaviour
    {
        private readonly List<Component> activeComponents = new List<Component>();
        public Image backgroundTextFieldOne;
        public Image backgroundTextFieldTwo;

        private MonoBehaviour currentQuestionnaire;
        private bool done;

        private OnlineResourceBehaviour extraResource;
        public Button extraResourceButton;
        private Component head;
        private bool isQuestionnaireActive;
        private IEnumerator<Component> iterator;

        public MultipleChoiceQuestionnaire multipleChoiceQuestionnaire;
        public GameObject quad;
        private readonly List<Component> removeComponents = new List<Component>();

        public TextMeshProUGUI textFieldOne;
        public TextMeshProUGUI textFieldTwo;

        public TextInit textInit;
        private float timer;

        public VideoPlayer videoPlayer;

        private void Reset()
        {
            Init();
        }

        private void Awake()
        {
            Init();
        }

        // Use this for initialization
        private void Start()
        {
            Init();
        }

        private void AddLeafWithText(Component head, float minute, float second, ILeafBehaviour behaviour)
        {
            Component textLeaf = new SmallEducatorLeaf(1001, "TextLeaf", minute, second, this, behaviour);
            head.AddComponent(textLeaf);
        }

        private TextTyper SetTextTyper(TextMeshProUGUI textField)
        {
            var textTyper = textField.GetComponent<TextTyper>();
            textTyper.TextField = textField;
            textTyper.TextSetting = new List<TextInit>();
            textTyper.TextSetting.Add(textInit);
            return textTyper;
        }

        private void Init()
        {
            UnityEngine.GUI.backgroundColor = Color.blue;
            head = new SmallEducatorComposite(0, "Head", -1.0f);

            TextTyper textTyper = null;
            textFieldOne = GameObject.FindGameObjectWithTag("TextFieldOne").GetComponent<TextMeshProUGUI>();
            textFieldTwo = GameObject.FindGameObjectWithTag("TextFieldTwo").GetComponent<TextMeshProUGUI>();
            backgroundTextFieldOne = GameObject.FindGameObjectWithTag("BackgroundTextFieldOne").GetComponent<Image>();
            backgroundTextFieldTwo = GameObject.FindGameObjectWithTag("BackgroundTextFieldTwo").GetComponent<Image>();
            extraResourceButton = GameObject.FindGameObjectWithTag("ExtraResourceButton").GetComponent<Button>();
            textTyper = SetTextTyper(textFieldOne);
            textTyper = SetTextTyper(textFieldTwo);

            LoadWeek2()
                ;
            //API test 
            //StartCoroutine(GetText());

            /*/Video
ILeafBehaviour textBehaviour = new VideoBehaviour(videoPlayer,
    "/Users/mjgth/Videos/Dm1hrYJX0AA8b5V.mp4"
    , false,
    //"https://video.twimg.com/tweet_video/DG8HO7UW0AAzsrL.mp4"
    //, true, 
    new Vector2(0, 0), 1024, 1024, quad);
//VideoBehaviour(, new Vector2(0, 0), 400.0f, 400.0f);
*/

            /*
        listOfLines.Clear();
        listOfLines.Add("hahaha ha hahahaha hahaha ha");
        listOfLines.Add("bla bla bla bla");
        listOfLines.Add("lalala lala lalalala lalala");

        //Typed text
        textBehaviour = new TextBehaviour(textTyper, listOfLines, new Vector2(-100, 100));
        addLeafWithText(head, 0.0f, 11.0f, textBehaviour);

        //ImageBehaviour(SmallEducator smallEducator, string url, GameObject quad,
        //Vector2 position, Texture2D tex, float timer)
        Texture2D tex = new Texture2D(680, 577, TextureFormat.DXT1, false);
        textBehaviour = new ImageBehaviour(this, "https://www.uml-diagrams.org/class-diagrams/class-diagram-domain-overview.png", quad, new Vector2(0, 0), tex, 10.0f);
        addLeafWithText(head, 0.0f, 11.0f, textBehaviour);
        */
            iterator = head.GetIterator();
            timer = 0.0f;
            done = false;
        }

        private void LoadWeek2()
        {
      
            var positionOnTimeLineSecondsTextOne = 0.0f;
            var positionOnTimeLineSeconds = 0.0f;
            var timeOnScreen = 10.0f;

            var listOfLinesTwo = new List<string>();
            var listOfLinesOne = new List<string>();

            var extraTextSettingsTwo = new ExtraTextSettings();
            var extraTextSettingsOne = new ExtraTextSettings();

            //SET Title (part one)
            listOfLinesTwo.Clear();
            listOfLinesTwo.Add("Welcome to Architecture and Design");

            extraTextSettingsTwo.HasBackGround = true;
            extraTextSettingsTwo.FontSize = 40;

            //Static text
            ILeafBehaviour textBehaviour = new StaticTextBehaviour(listOfLinesTwo, textFieldTwo,
                backgroundTextFieldTwo, extraTextSettingsTwo,
                timeOnScreen, new Vector2(0, 0));
            AddLeafWithText(head, Mathf.Floor(positionOnTimeLineSeconds / 60.0f), positionOnTimeLineSeconds % 60.0f,
                textBehaviour);

            //SET Sub title
            listOfLinesOne.Clear();
            listOfLinesOne.Add("Week 2 - Views");
            extraTextSettingsOne.FontSize = 20;
            extraTextSettingsOne.HasBackGround = true;

            positionOnTimeLineSeconds = SetStaticTextBehaviour(textFieldOne, backgroundTextFieldOne, listOfLinesOne,
                extraTextSettingsOne, timeOnScreen, positionOnTimeLineSeconds);


            AddMultipleChoiceQuestionnaire(positionOnTimeLineSeconds + 1.0f);

            //SET introduction of notation (part 2)

            listOfLinesOne.Clear();
            listOfLinesOne.Add("The architecture documentation should communicate the followings:");
            listOfLinesOne.Add("\t1. A big problem divided into smaller manageables ones.");
            listOfLinesOne.Add("\t2. Who is working on what and how to work together.");
            listOfLinesOne.Add("\t3. Provides a vocabulary for talking about a complex ideas.");
            listOfLinesOne.Add("\t4. The drives of the project.");
            listOfLinesOne.Add("\t5. Helps with avoiding costly mistakes.");
            listOfLinesOne.Add("\t6. It enables agility.");
            extraTextSettingsOne.FontSize = 20;
            extraTextSettingsOne.HasBackGround = true;
            timeOnScreen = 20.0f;

            positionOnTimeLineSecondsTextOne = SetStaticTextBehaviour(textFieldOne, backgroundTextFieldOne,
                listOfLinesOne,
                extraTextSettingsOne, timeOnScreen, positionOnTimeLineSeconds);

            listOfLinesOne.Clear();
            listOfLinesOne.Add("Diagrams with text helps to communicate the architecture.");
            listOfLinesOne.Add("There are different notations.");
            extraTextSettingsOne.FontSize = 20;
            extraTextSettingsOne.HasBackGround = true;
            timeOnScreen = 10.0f;

            positionOnTimeLineSecondsTextOne = SetStaticTextBehaviour(textFieldOne, backgroundTextFieldOne,
                listOfLinesOne,
                extraTextSettingsOne, timeOnScreen, positionOnTimeLineSecondsTextOne);

            //SET context of views (part 2.1.1)
            listOfLinesOne.Clear();
            listOfLinesOne.Add("Notations for architecture documentation");
            listOfLinesOne.Add("The notation for documenting an architecture can be Informal, Semiformal or Formal.");
            extraTextSettingsOne.FontSize = 20;
            extraTextSettingsOne.HasBackGround = true;

            positionOnTimeLineSecondsTextOne = SetStaticTextBehaviour(textFieldOne, backgroundTextFieldOne,
                listOfLinesOne,
                extraTextSettingsOne, timeOnScreen, positionOnTimeLineSecondsTextOne);

            //SET informal notation (part 2.2.1)
            var tex = new Texture2D(600, 374, TextureFormat.DXT1, false);
            ILeafBehaviour imageBehaviour0001 =
                new ImageFromResourcesBehaviour(this, "Textures/AaD/UML001", quad, new Vector2(0, 0), tex, 25.0f);
            AddLeafWithText(head, Mathf.Floor(positionOnTimeLineSecondsTextOne / 60.0f),
                positionOnTimeLineSecondsTextOne % 60.0f, imageBehaviour0001);

            listOfLinesOne.Clear();
            listOfLinesOne.Add("The informal notation is often used during meetings");
            listOfLinesOne.Add("or a discussion between software developers.");
            listOfLinesOne.Add("The diagrams are drawn on a white board or paper.");
            extraTextSettingsOne.FontSize = 20;
            extraTextSettingsOne.HasBackGround = true;

            positionOnTimeLineSecondsTextOne = SetStaticTextBehaviour(textFieldOne, backgroundTextFieldOne,
                listOfLinesOne,
                extraTextSettingsOne, timeOnScreen, positionOnTimeLineSecondsTextOne);

            //SET informal notation (part 2.2.2)
            listOfLinesOne.Clear();
            listOfLinesOne.Add("The informal notation is great for brainstorming");
            listOfLinesOne.Add("or for upfront design, but not for documenting an");
            listOfLinesOne.Add("architecture.");
            extraTextSettingsOne.FontSize = 20;
            extraTextSettingsOne.HasBackGround = true;

            positionOnTimeLineSecondsTextOne = SetStaticTextBehaviour(textFieldOne, backgroundTextFieldOne,
                listOfLinesOne,
                extraTextSettingsOne, timeOnScreen, positionOnTimeLineSecondsTextOne);

            //SET informal notation (part 2.2.3)
            listOfLinesOne.Clear();
            listOfLinesOne.Add("Don't use the informal notation when documenting the");
            listOfLinesOne.Add("architecture for the lab assignment.");
            extraTextSettingsOne.FontSize = 20;
            extraTextSettingsOne.HasBackGround = true;
            timeOnScreen = 5.0f;

            positionOnTimeLineSecondsTextOne = SetStaticTextBehaviour(textFieldOne, backgroundTextFieldOne,
                listOfLinesOne,
                extraTextSettingsOne, timeOnScreen, positionOnTimeLineSecondsTextOne);

            //SET semi-formal notation (part 2.3.1)
            tex = new Texture2D(1015, 770, TextureFormat.DXT1, false);
            ILeafBehaviour imageBehaviour0002 =
                new ImageFromResourcesBehaviour(this, "Textures/AaD/UML002", quad, new Vector2(0, 0), tex, 28.0f);
            AddLeafWithText(head, Mathf.Floor(positionOnTimeLineSecondsTextOne / 60.0f),
                positionOnTimeLineSecondsTextOne % 60.0f, imageBehaviour0002);

            extraResource = new OnlineResourceBehaviour("https://www.uml-diagrams.org/index-examples.html",
                extraResourceButton, 28.0f);
            AddLeafWithText(head, Mathf.Floor(positionOnTimeLineSecondsTextOne / 60.0f),
                positionOnTimeLineSecondsTextOne % 60.0f, extraResource);

            listOfLinesOne.Clear();
            listOfLinesOne.Add("The semi-formal notation is often used for design.");
            listOfLinesOne.Add("It is also used for discussion between software developers.");
            listOfLinesOne.Add("The diagrams are drawn using the UML notation.");
            extraTextSettingsOne.FontSize = 20;
            extraTextSettingsOne.HasBackGround = true;
            timeOnScreen = 10.0f;

            positionOnTimeLineSecondsTextOne = SetStaticTextBehaviour(textFieldOne, backgroundTextFieldOne,
                listOfLinesOne,
                extraTextSettingsOne, timeOnScreen, positionOnTimeLineSecondsTextOne);

            //SET semi-formal notation (part 2.3.2)
            listOfLinesOne.Clear();
            listOfLinesOne.Add("The semi-formal notation could be used for brainstorming");
            listOfLinesOne.Add("or for upfront design, but it is mostly used");
            listOfLinesOne.Add(" for documenting an architecture.");
            extraTextSettingsOne.FontSize = 20;
            extraTextSettingsOne.HasBackGround = true;

            positionOnTimeLineSecondsTextOne = SetStaticTextBehaviour(textFieldOne, backgroundTextFieldOne,
                listOfLinesOne,
                extraTextSettingsOne, timeOnScreen, positionOnTimeLineSecondsTextOne);

            //SET semi-formal notation (part 2.3.3)
            listOfLinesOne.Clear();
            listOfLinesOne.Add("Use the semi-formal notation when documenting the");
            listOfLinesOne.Add("architecture for the lab assignment.");
            extraTextSettingsOne.FontSize = 20;
            extraTextSettingsOne.HasBackGround = true;
            timeOnScreen = 5.0f;

            positionOnTimeLineSecondsTextOne = SetStaticTextBehaviour(textFieldOne, backgroundTextFieldOne,
                listOfLinesOne,
                extraTextSettingsOne, timeOnScreen, positionOnTimeLineSecondsTextOne);

            //SET semi-formal notation (part 2.4.1)
            //SET informal notation (part 2.2.1)
            timeOnScreen = 10.0f;
            tex = new Texture2D(1015, 770, TextureFormat.DXT1, false);
            ILeafBehaviour imageBehaviour0003 =
                new ImageFromResourcesBehaviour(this, "Textures/AaD/SpecMS", quad, new Vector2(0, 0), tex, 25.0f);
            AddLeafWithText(head, Mathf.Floor(positionOnTimeLineSecondsTextOne / 60.0f),
                positionOnTimeLineSecondsTextOne % 60.0f, imageBehaviour0003);

            listOfLinesOne.Clear();
            listOfLinesOne.Add("The formal notation is used for design, but these projects");
            listOfLinesOne.Add("are really large and takes years to development.");
            listOfLinesOne.Add("The formal notations are architecture description languages.");
            extraTextSettingsOne.FontSize = 20;
            extraTextSettingsOne.HasBackGround = true;

            positionOnTimeLineSecondsTextOne = SetStaticTextBehaviour(textFieldOne, backgroundTextFieldOne,
                listOfLinesOne,
                extraTextSettingsOne, timeOnScreen, positionOnTimeLineSecondsTextOne);

            //SET semi-formal notation (part 2.4.2)
            listOfLinesOne.Clear();
            listOfLinesOne.Add("The formal notation is used for upfront design");
            listOfLinesOne.Add("and it is used for documenting an architecture.");
            extraTextSettingsOne.FontSize = 20;
            extraTextSettingsOne.HasBackGround = true;

            positionOnTimeLineSecondsTextOne = SetStaticTextBehaviour(textFieldOne, backgroundTextFieldOne,
                listOfLinesOne,
                extraTextSettingsOne, timeOnScreen, positionOnTimeLineSecondsTextOne);

            //SET semi-formal notation (part 2.4.3)
            listOfLinesOne.Clear();
            listOfLinesOne.Add("Don't use the formal notation when documenting the");
            listOfLinesOne.Add("architecture for the lab assignment.");
            extraTextSettingsOne.FontSize = 20;
            extraTextSettingsOne.HasBackGround = true;
            timeOnScreen = 5.0f;

            positionOnTimeLineSecondsTextOne = SetStaticTextBehaviour(textFieldOne, backgroundTextFieldOne,
                listOfLinesOne,
                extraTextSettingsOne, timeOnScreen, positionOnTimeLineSecondsTextOne);

            //Step 6 views (part 2.5)
            listOfLinesOne.Clear();
            listOfLinesOne.Add("Views");
            listOfLinesOne.Add("For step 6 we use views to describe the architecture.");
            listOfLinesOne.Add(
                "First we will look a the views according to the book Software Architecture in Practice,");
            listOfLinesOne.Add("then view the views according to the book Applying UML and Patterns.");
            extraTextSettingsOne.FontSize = 20;
            extraTextSettingsOne.HasBackGround = true;
            timeOnScreen = 10.0f;

            positionOnTimeLineSecondsTextOne = SetStaticTextBehaviour(textFieldOne, backgroundTextFieldOne,
                listOfLinesOne,
                extraTextSettingsOne, timeOnScreen, positionOnTimeLineSecondsTextOne);

            listOfLinesOne.Clear();
            listOfLinesOne.Add("Views");
            listOfLinesOne.Add("“Thus, views let us divide the multidimensional entity that is a software");
            listOfLinesOne.Add("architecture into a number of (we hope) interesting and manageable");
            listOfLinesOne.Add("representations of the system. The concept of views gives us our most");
            listOfLinesOne.Add("fundamental principle of architecture documentation:\n");
            listOfLinesOne.Add("Documenting an architecture is a matter of documenting the relevant");
            listOfLinesOne.Add("views and then adding documentation that applies to more than one view.”\n");
            listOfLinesOne.Add("- Software Architecture in Practice");
            extraTextSettingsOne.FontSize = 20;
            extraTextSettingsOne.HasBackGround = true;
            timeOnScreen = 10.0f;

            positionOnTimeLineSecondsTextOne = SetStaticTextBehaviour(textFieldOne, backgroundTextFieldOne,
                listOfLinesOne,
                extraTextSettingsOne, timeOnScreen, positionOnTimeLineSecondsTextOne);

            //Texture2D 

            tex = new Texture2D(1079, 783, TextureFormat.DXT1, false);
            ILeafBehaviour imageBehaviour0004 =
                new ImageFromResourcesBehaviour(this, "Textures/AaD/Views001", quad, new Vector2(0, 0), tex, 25.0f);
            AddLeafWithText(head, Mathf.Floor(positionOnTimeLineSecondsTextOne / 60.0f),
                positionOnTimeLineSecondsTextOne % 60.0f, imageBehaviour0004);

            extraResource = new OnlineResourceBehaviour("https://www.uml-diagrams.org/", extraResourceButton, 30.0f);
            AddLeafWithText(head, Mathf.Floor(positionOnTimeLineSecondsTextOne / 60.0f),
                positionOnTimeLineSecondsTextOne % 60.0f, extraResource);

            listOfLinesOne.Clear();
            listOfLinesOne.Add("”Module structures exist at design time.");
            listOfLinesOne.Add("Module structures live on the file system and stick around");
            listOfLinesOne.Add("even when the software is not running.”");
            extraTextSettingsOne.FontSize = 20;
            extraTextSettingsOne.HasBackGround = true;
            timeOnScreen = 10.0f;

            setStaticTextBehaviour(textFieldOne, backgroundTextFieldOne, listOfLinesOne,
                extraTextSettingsOne, timeOnScreen, positionOnTimeLineSecondsTextOne, new Vector2(50.0f, 40.0f));

            listOfLinesTwo.Clear();
            listOfLinesTwo.Add("Design time is when program is being modelled and”");
            listOfLinesTwo.Add("it's code is being written.");
            extraTextSettingsTwo.FontSize = 20;
            extraTextSettingsTwo.HasBackGround = true;
            timeOnScreen = 10.0f;

            positionOnTimeLineSecondsTextOne = setStaticTextBehaviour(textFieldTwo, backgroundTextFieldTwo,
                listOfLinesTwo,
                extraTextSettingsTwo, timeOnScreen, positionOnTimeLineSecondsTextOne, new Vector2(100.0f, -55.0f));

            listOfLinesOne.Clear();
            listOfLinesOne.Add("”Component and connector, C&C, structures exist at runtime.");
            listOfLinesOne.Add("Component and connector structures don’t exist when the");
            listOfLinesOne.Add("software is not running.”");
            extraTextSettingsOne.FontSize = 20;
            extraTextSettingsOne.HasBackGround = true;
            timeOnScreen = 10.0f;

            setStaticTextBehaviour(textFieldOne, backgroundTextFieldOne, listOfLinesOne,
                extraTextSettingsOne, timeOnScreen, positionOnTimeLineSecondsTextOne, new Vector2(50.0f, 160.0f));

            listOfLinesTwo.Clear();
            listOfLinesTwo.Add("Run time is when program is being debugged, ”");
            listOfLinesTwo.Add("ran or is live on a server.");
            extraTextSettingsTwo.FontSize = 20;
            extraTextSettingsTwo.HasBackGround = true;
            timeOnScreen = 10.0f;

            positionOnTimeLineSecondsTextOne = setStaticTextBehaviour(textFieldTwo, backgroundTextFieldTwo,
                listOfLinesTwo,
                extraTextSettingsTwo, timeOnScreen, positionOnTimeLineSecondsTextOne, new Vector2(100.0f, -175.0f));

            listOfLinesTwo.Clear();
            listOfLinesTwo.Add(
                "“Allocation structures are created by showing\nhow modules and C&C elements correspond with each\nother and the physical elements that exist\nin real life.”");
            extraTextSettingsTwo.FontSize = 20;
            extraTextSettingsTwo.HasBackGround = true;
            timeOnScreen = 10.0f;

            positionOnTimeLineSecondsTextOne = setStaticTextBehaviour(textFieldTwo, backgroundTextFieldTwo,
                listOfLinesTwo,
                extraTextSettingsTwo, timeOnScreen, positionOnTimeLineSecondsTextOne, new Vector2(-95.0f, -45.0f));

            listOfLinesTwo.Clear();
            listOfLinesTwo.Add(
                "“Allocation structures are sometimes\ncalled mapping structures since they\nshow how different elements map to one another.”");
            extraTextSettingsTwo.FontSize = 20;
            extraTextSettingsTwo.HasBackGround = true;
            timeOnScreen = 10.0f;

            positionOnTimeLineSecondsTextOne = setStaticTextBehaviour(textFieldTwo, backgroundTextFieldTwo,
                listOfLinesTwo,
                extraTextSettingsTwo, timeOnScreen, positionOnTimeLineSecondsTextOne, new Vector2(-95.0f, -45.0f));
        }


        private void AddMultipleChoiceQuestionnaire(float positionOnTimeLine)
        {
            var anwsers = new List<string>();
            anwsers.Add("Yes");
            anwsers.Add("No");
            anwsers.Add("No..");

            anwsers.Add("Yes\nddddddddddddddddddddddsfsfsdf\ndfgdgdg");
            anwsers.Add("Yes\nddddddddddddddddddddddsfsfsdf\ndfgdgdg");
            anwsers.Add("No\nqwert\njyghf");
            anwsers.Add("No\nqwert\njyghf");

            var question =
                "Is dit een hele lange vraag om te beantwoorden zodat we de \ntextfield kunnen testen?\nYes it does!!";
            var singleAnswer = false;

            addMultipleChoiceQuestionnaire(positionOnTimeLine + 1.0f, anwsers, question, singleAnswer);
        }

        private void addMultipleChoiceQuestionnaire(float positionOnTimeLine, List<string> anwsers, string question,
            bool singleAnswer)
        {
            ILeafBehaviour choiceResource =
                new MultipleChoiceBehaviour(multipleChoiceQuestionnaire, anwsers, question, singleAnswer);
            AddLeafWithText(head, Mathf.Floor(positionOnTimeLine / 60.0f), positionOnTimeLine % 60.0f, choiceResource);
        }

        private float setStaticTextBehaviour(TextMeshProUGUI textField, Image backgroundTextField,
            List<string> listOfLines,
            ExtraTextSettings extraTextSettings, float timeOnScreen, float positionOnTimeLineSeconds, Vector2 position)
        {
            //Static text
            ILeafBehaviour textBehaviour = new StaticTextBehaviour(listOfLines, textField,
                backgroundTextField, extraTextSettings,
                timeOnScreen, position);
            AddLeafWithText(head, Mathf.Floor(positionOnTimeLineSeconds / 60.0f), positionOnTimeLineSeconds % 60.0f,
                textBehaviour);
            positionOnTimeLineSeconds += timeOnScreen + 0.30f;

            return positionOnTimeLineSeconds;
        }

        private float SetStaticTextBehaviour(TextMeshProUGUI textField, Image backgroundTextField,
            List<string> listOfLines,
            ExtraTextSettings extraTextSettings, float timeOnScreen, float positionOnTimeLineSeconds)
        {
            //Static text
            ILeafBehaviour textBehaviour = new StaticTextBehaviour(listOfLines, textField,
                backgroundTextField, extraTextSettings,
                timeOnScreen, new Vector2(0, -38));
            AddLeafWithText(head, Mathf.Floor(positionOnTimeLineSeconds / 60.0f), positionOnTimeLineSeconds % 60.0f,
                textBehaviour);
            positionOnTimeLineSeconds += timeOnScreen + 0.30f;

            return positionOnTimeLineSeconds;
        }

        public void RemoveComponent(Component componentToRemove)
        {
            removeComponents.Add(componentToRemove);
        }

        // Update is called once per frame
        public void FixedUpdate()
        {
            CleanUpActiveComponents();

            if (!isQuestionnaireActive)
            {
                CheckForNewActiveComponent();
                Play();
            }
        }

        private void CheckForNewActiveComponent()
        {
            //Debug.Log("Current.TimeStamp: " + iterator.Current.TimeStamp + " timer: " + timer);

            if (!done && iterator.Current.TimeStamp <= timer)
            {
                iterator.Current.Start();
                activeComponents.Add(iterator.Current);
                done = !iterator.MoveNext();
            }
        }

        private void Play()
        {
            foreach (var component in activeComponents) component.DoAction();
            timer += Time.fixedDeltaTime;
        }

        private void CleanUpActiveComponents()
        {
            if (removeComponents.Count > 0)
            {
                foreach (var component in removeComponents) activeComponents.Remove(component);
                removeComponents.Clear();
            }
        }

        public void QuestionnaireStarted(MonoBehaviour questionnaire)
        {
            currentQuestionnaire = questionnaire;
            isQuestionnaireActive = true;
        }

        public void QuestionnaireFinished()
        {
            currentQuestionnaire = null;
            isQuestionnaireActive = false;
        }

        public void ExtraResourceOnClick()
        {
            if (extraResource != null) extraResource.DoOnClick();
        }

        public void QuestionOnClick()
        {
            Debug.Log("Prompt the question box!!");
        }
        IEnumerator GetText()
        {
            UnityWebRequest www = UnityWebRequest.Get("http://www.my-server.com");
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                // Show results as text
                Debug.Log(www.downloadHandler.text);

                // Or retrieve results as binary data
                byte[] results = www.downloadHandler.data;
            }
        }
    }
}