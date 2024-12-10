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
    private int card1number = 0;
    private int card2number = 0;
    private int card3number = 0;
    private void Awake()
    {
        _cc = GetComponent<NetworkCharacterController>();
    }

    public override void FixedUpdateNetwork()
    {   
        if (GetInput(out NetworkInputData data))
        {
            velocity = data.velocity;
            angle += data.angle * angleMath;
            
            _cc.gameObject.transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y + angle, transform.rotation.z);
        }
        if (velocity > 0f)
            _cc.Move(transform.forward * Runner.DeltaTime);

        if (data.buttons.IsSet(NetworkInputData.EButton))
        {
            Debug.Log("using card" + card1number);
        }
    }

    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
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
        //whatever is supposed to happen
    }

    private void Card2()
    {
        //again
    }

    private void Card3()
    {
        //yeah
    }
}
