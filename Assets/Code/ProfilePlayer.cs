using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfilePlayer  
{
    public ProfilePlayer(float speedCar)
    {
        CurrentState = new SubscriptionProperty<GameState>();
        CurrentPlayerState = new SubscriptionProperty<PlayerState>();
        CurrentCar = new Car(speedCar);

        //Analytic = analytic;
    }

    public SubscriptionProperty<GameState> CurrentState { get; }
    public SubscriptionProperty<PlayerState> CurrentPlayerState { get; }

    public Car CurrentCar { get; }

   
}
