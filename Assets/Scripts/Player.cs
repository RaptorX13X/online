using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using TMPro;
using UnityEngine.UI;

public class Player : NetworkBehaviour
{
    private NetworkCharacterController _cc;
    private float velocity;
    private float angle;
    [SerializeField]private float angleMath;
    [SerializeField] private int score;
    [SerializeField]private int card1number = 0;
    [SerializeField]private int card2number = 0;
    [SerializeField]private int card3number = 0;
    [SerializeField]private bool doublePoints = false;
    [SerializeField]private bool slowdown = false;

    //[SerializeField] private int pointsToSteal = 10;
    
    [Networked] private TickTimer doubleTimer { get; set; }
    [Networked] private TickTimer slowTimer { get; set; }
    [SerializeField] private float doubleTimerInterval = 10f;
    [SerializeField] private float slowTimerInterval = 10f;

    [SerializeField] private Image card1Image;
    [SerializeField] private Image card2Image;
    [SerializeField] private Image card3Image;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private TextMeshProUGUI text2;
    [SerializeField] private TextMeshProUGUI text3;

    [SerializeField] private Sprite card1Sprite;
    [SerializeField] private Sprite card2Sprite;
    [SerializeField] private Sprite emptySprite;

    private NetworkObject networkObject;
    private void Awake()
    {
        _cc = GetComponent<NetworkCharacterController>();
        networkObject = GetComponent<NetworkObject>();
    }

    public override void FixedUpdateNetwork()
    {
        if (slowdown)
        {
            _cc.maxSpeed = 6f;
        }
        else
        {
            _cc.maxSpeed = 3f;
        }
        if (GetInput(out NetworkInputData data))
        {
            velocity = data.velocity;
            if (slowdown) velocity *= 1.5f;
            angle += data.angle * angleMath;
            
            _cc.gameObject.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x,  angle, transform.rotation.eulerAngles.z);
        }
        if (velocity > 0f)
            _cc.Move(transform.forward * Runner.DeltaTime);

        if (data.buttons.IsSet(NetworkInputData.EButton))
        {
            UseCard();
        }
        if (doubleTimer.Expired(Runner))
        {
            doublePoints = false;
            text2.text = null;
        }

        if (slowTimer.Expired(Runner))
        {
            slowdown = false;
            text3.text = null;
        }
        
        Debug.Log(networkObject.InputAuthority);
    }

    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
        if (doublePoints)
        {
            score += scoreToAdd;
        }
        text.text = "Score:" + score;
    }

    public void AddCard(int cardID)
    {
        Sprite sprite = null;
        if (cardID == 1)
        {
            sprite = card1Sprite;
        }
        else if (cardID == 2)
        {
            sprite = card2Sprite;
        }
        
        if (card1number == 0)
        {
            card1number = cardID;
            card1Image.sprite = sprite;
        }
        else if (card2number == 0)
        {
            card2number = cardID;
            card2Image.sprite = sprite;
        }
        else if (card3number == 0)
        {
            card3number = cardID;
            card3Image.sprite = sprite;
        }
        else return;
    }

    private void UseCard()
    {
        if (card1number == 0) 
            NoCards();
        else if (card1number == 1) 
            Card1();
        else if (card1number == 2) 
            Card2();
        // else if (card1number == 3) 
        //     Card3();
        card1Image.sprite = emptySprite;
        card1number = 0;
        if (card2number != 0)
        {
            card1number = card2number;
            card1Image.sprite = card2Image.sprite;
        }

        if (card3number != 0)
        {
            card2number = card3number;
            card2Image.sprite = card3Image.sprite;
            card3number = 0;
            card3Image.sprite = emptySprite;
        }
        else
        {
            card2number = 0;
            card2Image.sprite = emptySprite;
        }
    }

    private void NoCards()
    {
        Debug.Log("no cards to use");
    }

    private void Card1()
    {
        doubleTimer = TickTimer.CreateFromSeconds(Runner, doubleTimerInterval);
        text2.text = "Double Points!";
        doublePoints = true;
    }

    private void Card2()
    {
        slowTimer = TickTimer.CreateFromSeconds(Runner, slowTimerInterval);
        text3.text = "Speed Up!";
        slowdown = true;
    }

    /*private void Card3()
    {
        StealPoints();
    }

    private void StealPoints()
    {
        if (networkObject.InputAuthority == PlayerRef.FromIndex(1))
        {
            Runner.GetPlayerObject(PlayerRef.FromIndex(2)).GetComponent<Player>().Stolen(pointsToSteal);
            score += pointsToSteal;
        }
        else if (networkObject.InputAuthority == PlayerRef.FromIndex(2))
        {
            Runner.GetPlayerObject(PlayerRef.FromIndex(1)).GetComponent<Player>().Stolen(pointsToSteal);
            score += pointsToSteal;
        }
    }

    public void Stolen(int points)
    {
        score -= points;
    }*/
}
