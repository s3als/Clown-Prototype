using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClownBattle : MonoBehaviour
{
    // data, tweakables ------------------------

    //clown moves
    Setup slapstickSetup = new Setup(HT.Slapstick, 3);
    Setup lowbrowSetup = new Setup(HT.Lowbrow, 3);
    Setup highbrowSetup = new Setup(HT.Highbrow, 3);

    Payoff slapstickPayoff = new Payoff(HT.Slapstick, 3);
    Payoff lowbrowPayoff = new Payoff(HT.Lowbrow, 3);
    Payoff highbrowPayoff = new Payoff(HT.Highbrow, 3);

    Riff highbrowRiff = new Riff(HT.Highbrow, 1);
    Riff lowbrowRiff = new Riff(HT.Lowbrow, 1);
    Riff slapstickRiff = new Riff(HT.Slapstick, 1);

    //audience moves
    Setup slapstickHeckle = new Setup(HT.Slapstick, 1);
    Setup lowbrowHeckle = new Setup(HT.Lowbrow, 2);
    Setup highbrowHeckle = new Setup(HT.Highbrow, 3);

    //which clowns have which moves
    void CreateClowns()
    {
        clowns = new List<Clown>();

        Clown hedwig = new Clown("Hedwig");
        hedwig.actions.Add(lowbrowRiff);
        hedwig.actions.Add(highbrowRiff);
        hedwig.actions.Add(highbrowPayoff);
        clowns.Add(hedwig);

        Clown yves = new Clown("Yves");
        yves.actions.Add(slapstickSetup);
        yves.actions.Add(lowbrowPayoff);
        yves.actions.Add(highbrowSetup);
        clowns.Add(yves);

        Clown yvonne = new Clown("Yvonne");
        yvonne.actions.Add(slapstickPayoff);
        yvonne.actions.Add(lowbrowSetup);
        yvonne.actions.Add(slapstickRiff);
        clowns.Add(yvonne);

    }

    //which audience members have which moves
    void CreateAudience()
    {
        audience = new List<AudienceMember>();

        AudienceMember lowbrowEnjoyer = new AudienceMember("Lowbrow Enjoyer", 1, 0.1f, new List<HT> { HT.Lowbrow });
        lowbrowEnjoyer.actions.Add(lowbrowHeckle);
        audience.Add(lowbrowEnjoyer);

        AudienceMember slapstickEnjoyer = new AudienceMember("Slapstick Enjoyer", 1, 0.1f, new List<HT> { HT.Slapstick });
        lowbrowEnjoyer.actions.Add(slapstickHeckle);
        audience.Add(slapstickEnjoyer);

        AudienceMember highbrowEnjoyer = new AudienceMember("Highbrow Enjoyer", 1, 0.1f, new List<HT> { HT.Highbrow });
        highbrowEnjoyer.actions.Add(highbrowHeckle);
        audience.Add(highbrowEnjoyer);
    }

    // logic ------------------------------------------

    public List<Clown> clowns;
    public List<AudienceMember> audience;
    Performance performance;

    public List<Action> actions;

    bool waitForInput = false;
    bool canAdvanceTurn = false;

    public TextMeshProUGUI clownName1;
    public TextMeshProUGUI clownName2;
    public TextMeshProUGUI clownName3;

    public TextMeshProUGUI audienceName1;
    public TextMeshProUGUI audienceName2;
    public TextMeshProUGUI audienceName3;

    public TextMeshProUGUI audienceStats1;
    public TextMeshProUGUI audienceStats2;
    public TextMeshProUGUI audienceStats3;

    public TextMeshProUGUI setup1;
    public TextMeshProUGUI setup2;
    public TextMeshProUGUI setup3;

    public TextMeshProUGUI textBox;

    public TextMeshProUGUI continuePrompt;

    void Start()
    {
        CreateClowns();
        CreateAudience();
        performance = new Performance(clowns, audience, 10);
        UpdateGUI(performance);
        textBox.text = "";
        SetContinuePrompt(false);
    }
    
    void Update()
    {
        if (performance.IsNextAlert())
        {
            SetContinuePrompt(true);
            if (GetInput() != null)
            {
                NewText(performance.PullNextAlert());
                SetContinuePrompt(false);
                return;
            }
            return;
        }
        else
        {
            if (waitForInput)
            {
                switch (GetInput())
                {
                    case "space":
                        waitForInput = false;
                        canAdvanceTurn = true;
                        UpdateGUI(performance);
                        performance.Alert("Skipped turn.");
                        NewText(performance.PullNextAlert());
                        break;
                    case "1":
                        if (performance.SelectCurrentPerformerAction(0))
                        {
                            canAdvanceTurn = true;
                        }
                        waitForInput = false;
                        UpdateGUI(performance);
                        NewText(performance.PullNextAlert());
                        break;
                    case "2":
                        if (performance.SelectCurrentPerformerAction(1))
                        {
                            canAdvanceTurn = true;
                        }
                        waitForInput = false;
                        UpdateGUI(performance);
                        NewText(performance.PullNextAlert());
                        break;
                    case "3":
                        if (performance.SelectCurrentPerformerAction(2))
                        {
                            canAdvanceTurn = true;
                        }
                        waitForInput = false;
                        UpdateGUI(performance);
                        NewText(performance.PullNextAlert());
                        break;
                    case null:
                        break;
                }
            }
            else if(canAdvanceTurn) 
            {
                canAdvanceTurn = false;
                waitForInput = false;
                performance.AdvancePerformance();
                UpdateGUI(performance);
            }
            else
            {
                performance.ShowCurrentPerformerActions();
                UpdateGUI(performance);
                waitForInput = true;
            }
        }
    }
    
    string GetInput()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            return "space";
        }
        else if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            return "1";
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            return "2";
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            return "3";
        }
        return null;
    }

    void UpdateGUI(Performance p)
    {
        clownName1.text = p.performers[0].name;
        if(p.performers[1] != null)
        {
            clownName2.text = p.performers[1].name;
        }
        if(p.performers[2] != null)
        {
            clownName3.text = p.performers[2].name;
        }

        audienceName1.text = p.audience[0].name;
        audienceStats1.text = "Mirth " + p.audience[0].mirth + "\nAttn " + p.audience[0].attention;
        if (p.audience.Count > 1)
        {
            audienceName2.text = p.audience[1].name;
            audienceStats2.text = "Mirth " + p.audience[1].mirth + "\nAttn " + p.audience[1].attention;
        }
        else { audienceName2.text = ""; audienceStats2.text = ""; }
        if (p.audience.Count > 2)
        {
            audienceName3.text = p.audience[2].name;
            audienceStats3.text = "Mirth " + p.audience[2].mirth + "\nAttn " + p.audience[2].attention;
        }
        else { audienceName3.text = ""; audienceStats3.text = ""; }

        if (p.setupTypes[0] != HT.none)
        {
            setup1.text = p.setupTypes[0].ToString() + " " + p.setupDurations[0];
        }
        else setup1.text = "-";
        if (p.setupTypes[1] != HT.none)
        {
            setup2.text = p.setupTypes[1].ToString() + " " + p.setupDurations[1];
        }
        else setup2.text = "-";
        if (p.setupTypes[2] != HT.none)
        {
            setup3.text = p.setupTypes[2].ToString() + " " + p.setupDurations[2];
        }
        else setup3.text = "-";
    }

    void NewText(string text)
    {
        textBox.text = text;
    }

    void SetContinuePrompt(bool state)
    {
        continuePrompt.enabled = state;
    }
}

public enum HT
{
    none,
    Lowbrow, //low-brow
    Highbrow, //high-brow
    Slapstick, //slapstick
    Dry, //dry
    Political //political
}

public class Performance
{
    public List<Clown> performers;
    public List<AudienceMember> audience;
    public int rounds;
    int setups;
    public HT[] setupTypes;
    public int[] setupDurations;
    int turn; 
    
    public List<string> alerts;
    int alertIndex = -1;

    public Performance(List<Clown> performers, List<AudienceMember> audience, int rounds)
    {
        alerts = new List<string>();
        this.performers = performers;
        this.audience = audience;
        this.rounds = rounds;
        setups = 0;
        turn = 0;
        setupTypes = new HT[3];
        setupDurations = new int[3];
        //performanceText = "";
    }

    //Are there three setups?
    public bool SetupQueueFull()
    {
        if(setups == 3)
        {
            return true;
        }
        return false;
    }

    //Add a setup to the queue.
    public void AddSetup(Setup setup)
    {
        if(setups == 3)
        {
            int index = Random.Range(0, 2);
            setupTypes[index] = setup.type;
            setupDurations[index] = setup.time;
        }
        setupTypes[setups] = setup.type;
        setupDurations[setups] = setup.time;
        setups++;
        //Alert("Added a new Setup.");
        //performanceText = "Added setup " + setup.type.ToString() + " with timer " + setup.time;
        //Debug.Log("Added setup " + setup.type.ToString() + " with timer " + setup.time);
    }

    //Advances the turn, advances round if all turns spent. 
    //Modifies all time-specific numbers (setup duration, attention, mirth)
    //Calls AudienceActPhase once advancement is done.
    public void AdvancePerformance()
    {
        turn++;
        if (turn == performers.Count)
        {
            turn = 0;
            rounds--;
            if(rounds == 0)
            {
                Alert("Performance over");
                return;
            }
            foreach (AudienceMember audienceMember in audience)
            {
                audienceMember.attention += audienceMember.mirth;
                audienceMember.mirth--;
            }
            Alert("Begin Round " + rounds);
            for (int i = 0; i < 3; i++)
            {
                if (setupTypes[i] != HT.none)
                {
                    setupDurations[i]--;
                }
            }
        }
        AudienceActPhase();
    }

    //Calculate the payoff of the setup at setupIndex with potency payoffPotency.
    //Apply to audience.
    public void Payoff(int setupIndex, int payoffPotency)
    {
        int result = 0;
        int distance = Mathf.Abs(setupDurations[setupIndex]);
        if (distance == 0) { result = payoffPotency; }
        else if (distance == 1) { result = payoffPotency / 2; }
        else if (distance == 2) { result = payoffPotency / 4; }
        else result = 0;
        AffectAudience(setupTypes[setupIndex], result);
        setupDurations[setupIndex] = -1;
        setupTypes[setupIndex] = HT.none;
        setups--;
    }

    //Attempt to perform the action at index actionIndex for the performer whose turn it is.
    public bool SelectCurrentPerformerAction(int actionIndex)
    {
        Action selectedAction = performers[turn].actions[actionIndex];
        
        if (selectedAction is Setup)
        {
            Setup setup = (Setup)selectedAction;
            if(SetupQueueFull())
            {
                Alert("Setup queue is full.");
                return false;
            }
            Alert("Set up a " + setup.type + " joke.");
            AddSetup(setup);
            return true;
        }
        else if (selectedAction is Payoff)
        {
            Payoff payoff = (Payoff)selectedAction;
            int lowestTime = 100;
            int index = -1;
            for(int i = 0; i < setups; i++)
            {
                if (setupTypes[i] == payoff.type)
                {
                    if(setupDurations[i] < lowestTime)
                    {
                        lowestTime = setupDurations[i];
                        index = i;
                    }
                }
            }
            if(index != -1)
            {
                Alert("Paid off " + setupTypes[index] + " " + setupDurations[index]);
                Payoff(index, payoff.potency);
                return true;
            }
            Alert("No setups of type " + payoff.type + ".");
            return false;
        }
        else if(selectedAction is Riff)
        {
            Riff riff = (Riff)selectedAction;
            Alert("Performed riff " + riff.name);
            AffectAudience(riff.type, riff.potency);
            return true;
        }
        return false;
    }

    //Roll for the audience members to see if they act. 
    public bool AudienceActPhase()
    {
        foreach(AudienceMember audienceMember in audience)
        {
            if (RollForAudienceMemberAction(audienceMember)) { return true; }
        }
        return false;
    }
    
    //Roll an individual audience member's action chance.
    public bool RollForAudienceMemberAction(AudienceMember audienceMember)
    {
        if (Random.value < audienceMember.actionChance)
        {
            string thisAlert = "Audience acts! ";
            Action chosen = audienceMember.actions[Random.Range(0, audienceMember.actions.Count)];
            if(chosen is Setup)
            {
                AddSetup((Setup)chosen);
                thisAlert += " Heckled: " + chosen.name;
            }
            Alert(thisAlert);
            return true;
        }
        return false;
    }

    //Calculate the effect on the audience with a given type and potency.
    public void AffectAudience(HT type, int potency)
    {
        foreach(AudienceMember audienceMember in audience)
        {
            if(audienceMember.senseOfHumor.Contains(type))
            {
                audienceMember.mirth += potency;
                Alert(audienceMember.name + " hit for " + potency + " mirth!");
            }
            else
            {
                Alert(audienceMember.name + " not affected.");
            }
        }
    }

    //Deploy an alert showing the actions of the performer whose turn it is.
    public void ShowCurrentPerformerActions()
    {
        string thisAlert = performers[turn].name + "'s turn - select move";
        int index = 1;
        foreach (Action action in performers[turn].actions)
        {
            thisAlert += "\n" + index + ": " + action.name;
            index++;
        }
        Alert(thisAlert);
    }

    //Add an Alert to  the queue.
    public void Alert(string alert)
    {
        Debug.Log(alert + "\n(alert count " + alerts.Count + ")");
        alerts.Add(alert);
    }

    //Is there an Alert left?
    public bool IsNextAlert()
    {
        if(alertIndex + 1 == alerts.Count)
        {
            return false;
        }
        return true;
    }

    //Give me the next Alert.
    public string PullNextAlert()
    {
        alertIndex++;
        Debug.Log("pulled alert " + alertIndex);
        return alerts[alertIndex];
    }
}

public class Clown
{
    public List<Action> actions;
    new public string name;

    public Clown(string name)
    {
        this.name = name;
        actions = new List<Action>();

    }
}

public abstract class Action 
{
    public string name;
    public void Perform() { }
}

public class Setup : Action
{
    public Setup(HT type, int time)
    {
        this.type = type;
        this.time = time;
        name = type.ToString() + " Setup, " + time + " turns";
    }
    public HT type;
    public int time;
}

public class Payoff : Action
{
    public Payoff(HT type, int potency)
    {
        this.type = type;
        this.potency = potency;
        name = type.ToString() + " Payoff, " + potency + " power";
    }
    public HT type;
    public int potency;
}

public class Riff : Action
{
    public Riff(HT type, int potency)
    {
        this.type = type;
        this.potency = potency;
        name = type.ToString() + " Riff, " + potency + " power";
    }
    public HT type;
    public int potency;
}

public class AudienceMember
{
    public string name;
    public int attention;
    public int mirth;
    public int resistance;
    public float actionChance;
    public List<HT> senseOfHumor;
    public List<Action> actions;

    public AudienceMember(string name, int resistance, float actionChance, List<HT> senseOfHumor)
    {
        actions = new List<Action>();
        this.name = name;
        attention = 10;
        mirth = 0;
        this.actionChance = actionChance;
        this.resistance = resistance;
        this.senseOfHumor = senseOfHumor;
    }
}