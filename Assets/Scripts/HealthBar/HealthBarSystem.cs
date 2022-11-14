using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HealthBarSystem
{
    private List<Heart> heartList;

    public HealthBarSystem(int amount)
    {
        heartList = new List<Heart>();
        for(int i = 0; i < amount; i++)
        {
            heartList.Add(new Heart(true));
        }
    }

    public List<Heart> GetHeartList()
    {
        return heartList;
    }

    public void Damage()
    {
        //On parcours chaque coeur en commençant par le dernier
        for(int i = heartList.Count - 1; i>=0; i--)
        {
            //Si il est plein, on le vide
            if (heartList[i].GetState())
            {
                heartList[i].SetState(false);
                break;
            }
        }
    }

    public class Heart
    {
        private bool isFull;

        public Heart(bool isFull)
        {
            this.isFull = isFull;
        }

        public bool GetState()
        {
            return isFull;
        }

        public void SetState(bool state)
        {
            isFull = state;
        }
    }
}
