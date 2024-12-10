using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

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

    [SerializeField] private int pointsToSteal = 10;
    
    [Networked] private TickTimer doubleTimer { get; set; }
    [Networked] private TickTimer slowTimer { get; set; }
    [SerializeField] private float doubleTimerInterval = 10f;
    [SerializeField] private float slowTimerInterval = 10f;

    private NetworkObject networkObject;
    private void Awake()
    {
        _cc = GetComponent<NetworkCharacterController>();
        networkObject = GetComponent<NetworkObject>();
    }

    public override void FixedUpdateNetwork()
    {   
        if (GetInput(out NetworkInputData data))
        {
            velocity = data.velocity;
            if (slowdown) velocity *= 0.75f;
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
    }

    public void AddCard(int cardID)
    {
        if (card1number == 0)
            card1number = cardID;
        else if (card2number == 0)
            card2number = cardID;
        else if (card3number == 0)
            card3number = cardID;
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
        else if (card1number == 3) 
            Card3();

        card1number = 0;
        if (card2number != 0)
        {
            card1number = card2number;
        }

        if (card3number != 0)
        {
            card2number = card3number;
            card3number = 0;
        }
        else card2number = 0;
    }

    private void NoCards()
    {
        Debug.Log("no cards to use");
    }

    private void Card1()
    {
        doubleTimer = TickTimer.CreateFromSeconds(Runner, doubleTimerInterval);
        doublePoints = true;
    }

    private void Card2()
    {
        if (networkObject.InputAuthority == PlayerRef.FromIndex(1))
        {
            Runner.GetPlayerObject(PlayerRef.FromIndex(2)).GetComponent<Player>().SlowDown();
        }
        else if (networkObject.InputAuthority == PlayerRef.FromIndex(2))
        {
            Runner.GetPlayerObject(PlayerRef.FromIndex(1)).GetComponent<Player>().SlowDown();
        }
    }

    private void Card3()
    {
        StealPoints();
    }

    public void SlowDown()
    {
        slowTimer = TickTimer.CreateFromSeconds(Runner, slowTimerInterval);
        slowdown = true;
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
    }
}
