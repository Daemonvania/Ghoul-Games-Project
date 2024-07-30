using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlantStateManager : Interactable
{


    [SerializeField, Range(5, 20)] private float difficultyLevel = 5;
    [Space] public PlantBaseState currentState;

    //todo make this better, can be managed by diff script or jus diff way    
    [SerializeField] public TMP_Text _text;

    [SerializeField] public TMP_Text shedNeedText;
    [SerializeField] public TMP_Text shakeNeedText;
    public ParticleSystem shedParticle;


    //Could make it an array..?
    public PlantStateIdle idleState = new PlantStateIdle();
    public PlantStateShed sheddingState = new PlantStateShed();
    public PlantStateShake shakingState = new PlantStateShake();
    public PlantStateAngry angryState = new PlantStateAngry();


    //variables
    [HideInInspector] public bool jumpScare = false;
    [HideInInspector] public GameObject jumpScareCanvas;
    [HideInInspector] public Animator _animator;

    [HideInInspector] public bool isLit = false;

    [HideInInspector] public int angerLevel = 0;
    private float lightTimer = 0;
    
    //todo make this manage needs and then pass them on on start, so that they can be unique for each state, this could also allow having more common functionality
    //here, instead of having to repeat it in each state

    private void Awake()
    {
        jumpScareCanvas = GameObject.FindWithTag("JumpScareCanvas");
    }

    private void Start()
    {
        base.Start();

        jumpScareCanvas.SetActive(false);
        idleState.Initialize(this);
        sheddingState.Initialize(this);
        shakingState.Initialize(this);
        angryState.Initialize(this);
        _animator = GetComponentInChildren<Animator>();

        //todo set a lot of this up for the editor 
        InvokeRepeating(nameof(ProgressCheck), Random.Range(4, 6), Random.Range(13, 16));

        InvokeRepeating(nameof(IncreaseDifficulty), 10, 60);
        
        
        currentState = idleState;
        currentState.EnterState(this);
    }

    void IncreaseDifficulty()
    {
        difficultyLevel++;
        Debug.Log("difficulty Level:" + difficultyLevel);
    }


    void Update()
    {
        currentState.Update(this);
        _text.text = angerLevel.ToString();
        if (isLit)
        {
            Debug.Log("isLit");
            lightTimer += Time.deltaTime;
            
            angerLevel = Mathf.Clamp(angerLevel, 0, 2);
            
            if (lightTimer >= 5)
            {
                Debug.Log("goingIdle");
                currentState = idleState;
                currentState.EnterState(this);
            }
        }
    }

    public void ProgressCheck()
    {
        Debug.Log("ProgressCheck");
        //change this dynamically as well
        if (Random.Range(0, 20) < difficultyLevel)
        {
            currentState.Progress(this);
        }
    }

    public void LightChange(bool lit)
    {
        isLit = lit;
        lightTimer = 0;
    }

    public void OnToolUse(ToolType selectedTool)
    {
        currentState.ToolUsed(this, selectedTool);

    }
    public override void Interact(Player player)
    {
    }

    public override void OnLook()
    {
        
    }
}
