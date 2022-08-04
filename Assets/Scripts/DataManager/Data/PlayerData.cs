using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// 用户信息类(实时)
/// </summary>
[Serializable]
public class PlayerData : Data
{

    public int level = 1;

    public int attack=10;

    public int hp=100;

    public int mp=80;

    public int Coin = 100;

    public int exp = 0;
    public PlayerData()
    {
    }
    public PlayerData(int level,int attack,int hp,int mp,int coin,int exp)
    {
        this.level = level;
        this.attack = attack;
        this.hp = hp;
        this.mp = mp;
        this.Coin = coin;
        this.exp = exp;
    }
}

/// <summary>
/// 用户信息类(最大)
/// </summary>
[Serializable]
public class MaxPlayerData : Data
{
    public int attack=10;

    public int hp = 100;

    public int mp = 80;

    public int exp = 300;
    public MaxPlayerData()
    {
    }
    public MaxPlayerData(int attack, int hp, int mp,int exp)
    {
        this.attack = attack;
        this.hp = hp;
        this.mp = mp;
        this.exp = exp;
    }
}