using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneStateManager
{
    public MainSenceState mainSenceState;
    public BossGameOptionSenceState bossGameOptionSenceState;
    public NormalGameOptionSenceState normalGameOptionSenceState;
    public BossModelSenceState bossModelSenceState;
    public MonsterNestSenceState monsterNestSenceState;
    public NomalModelSenceState nomalModelSenceState;
    public StartLoadSenceState startLoadSenceState;
    public LoginSenceState loginSenceState;
    public SceneStateManager()
    {
        mainSenceState = new MainSenceState();
        bossGameOptionSenceState = new BossGameOptionSenceState();
        normalGameOptionSenceState = new NormalGameOptionSenceState();
        bossGameOptionSenceState = new BossGameOptionSenceState();
        monsterNestSenceState = new MonsterNestSenceState();
        nomalModelSenceState = new NomalModelSenceState();
        startLoadSenceState = new StartLoadSenceState();
        loginSenceState=new LoginSenceState();
    }
}
