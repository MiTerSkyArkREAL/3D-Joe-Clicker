//TODO:
//Parkour course; prize triples current amount of money
//Upgrade that lowers cost scaler
//Add saving when exiting
//Anvil that generates hiring managers
using UnityEngine;
using Unity;
using System;
using TMPro;

[RequireComponent(typeof(Rigidbody))]
public class InteractionScript : MonoBehaviour
{

    [SerializeField] private AudioClip BassSound;

    private AudioSource audioSource;

    public float jumpForce = 2f;
    private bool isGrounded = true;
    private Rigidbody rb;
    public Transform PlayerObject;
    public Transform Table;
    public Transform Wall;
    public Transform Lemon;
    public Transform JobApp;
    public Transform Worker;
    public Transform HManApp;
    public Transform SecretWall;
    public Transform SecretWall2;
    public Transform Gun;
    public GameObject PlayerGun;
    public Transform Bass;
    public Transform Anvil;

    public float costScaler = 1.2f;

    public float Money = 0f;
    public TMP_Text MoneyText;
    public TMP_Text WallCostText;
    public TMP_Text HighQualLemonText;
    public TMP_Text WorkerText;
    public TMP_Text HireManText;
    public TMP_Text AnvilText;

    public bool wallBought = false;
    public float highQualLems = 0f;
    public float HighQualLemCost = 15f;
    public float Workers = 0f;
    public float WorkerCost = 500f;
    public float WorkerMult = 1f;
    public float HireMans = 0f;
    public float HireManMult = 1;
    public float HireManCost = 6000;
    public float HMForgers = 0;
    public float HMForgerCost = 500000;

    public bool Secret1Moved = false;
    public bool Secret2Moved = false;

    public bool gun = false;

    DayOfWeek today = DateTime.Now.DayOfWeek;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        PlayerGun.SetActive(false);

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (DateTime.Now.DayOfWeek != DayOfWeek.Sunday){
            if(!Secret1Moved){
                Secret1Moved = true;
            for (int i = 3; i > 0; i--)
                SecretWall.position += Vector3.down;
            }
        }else if(Secret1Moved){
            Secret1Moved = false;
            for (int i = 3; i > 0; i--)
                SecretWall.position += Vector3.up;
        }
        if (DateTime.Now.DayOfWeek == DayOfWeek.Tuesday){
            if(!Secret2Moved){
                Secret2Moved = true;
            for (int i = 3; i > 0; i--)
                SecretWall2.position += Vector3.down;
            }
        }else if(Secret2Moved){
            Secret2Moved = false;
            for (int i = 3; i > 0; i--)
                SecretWall2.position += Vector3.up;
        }

        MoneyText.text = "Money: $" + Money.ToString("F2");
        WorkerText.text = "Hire Worker ($" + WorkerCost + ")" + "\n" + "[E]" + "\n" + Workers + ", " + WorkerMult;
        HireManText.text = "Hire Hiring Manager ($" + HireManCost + ")" + "\n" + "[E]" + "\n" + HireMans + ", " + HireManMult;


        if (Workers > 0)
        {
            if (highQualLems > 2)
            {
                Money += Workers * WorkerMult * (highQualLems - 2) * Time.deltaTime;
            }
            else
            {
                Money += Workers * WorkerMult * Time.deltaTime;
            }
        }

        if (HireMans > 0)
        {
            Workers += HireMans * WorkerMult * 0.25f * Time.deltaTime;
        }

        if (HMForgers > 0)
        {
            HireMans += HMForgers * HireManMult * 0.25f * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }

        if (PlayerObject != null && Bass != null)
        {
            float distance = Vector3.Distance(PlayerObject.position, Bass.position);
            if (distance < 5 && Input.GetKeyDown(KeyCode.E))
            {
                if(gun){
                    Money = Money * 1.005f;
                }

                audioSource.clip = BassSound; 
                audioSource.Play();
            }
        }

        if (PlayerObject != null && Gun != null)
        {
            float distance = Vector3.Distance(PlayerObject.position, Gun.position);
            if (distance < 5 && Input.GetKeyDown(KeyCode.E))
            {
                gun = true;
                PlayerGun.SetActive(true);
                for (int i = 3; i > 0; i--)
                Gun.position += Vector3.down;
            }
        }

        if (PlayerObject != null && Table != null)
        {
            float distance = Vector3.Distance(PlayerObject.position, Table.position);
            if (distance < 3.5 && Input.GetKeyDown(KeyCode.E))
            {
                Money = Money + 1 + highQualLems;
                MoneyText.text = "Money: $" + Money.ToString("F2");
            }
        }

        if (PlayerObject != null && Wall != null)
        {
            float distance = Vector3.Distance(PlayerObject.position, Wall.position);
            if (distance < 3.5 && Input.GetKeyDown(KeyCode.E) && Money >= 100 && !wallBought)
            {
                Money = Money - 100;
                MoneyText.text = "Money: $" + Money.ToString("F2");
                jumpForce = 4f;
                WallCostText.text = "Jump Higher";
                wallBought = true;
                for (int i = 2; i > 0; i--)
                {
                    Wall.position += Vector3.down;
                }
            }
        }

        if (PlayerObject != null && Lemon != null)
        {
            float distance = Vector3.Distance(PlayerObject.position, Lemon.position);
            if (distance < 3.7 && Input.GetKeyDown(KeyCode.E) && Money >= HighQualLemCost)
            {
                buyHighQualLemon();
                MoneyText.text = "Money: $" + Money.ToString("F2");
                HighQualLemonText.text = "Higher Quality Lemons ($" + HighQualLemCost + ")" + "\n" + "[E]" + "\n" + highQualLems;
            }
        }

        if (PlayerObject != null && JobApp != null)
        {
            float distance = Vector3.Distance(PlayerObject.position, JobApp.position);
            if (distance < 3.6 && Input.GetKeyDown(KeyCode.E) && Money >= WorkerCost)
            {
                hireWorker();
                MoneyText.text = "Money: $" + Money.ToString("F2");
                WorkerText.text = "Hire Worker ($" + WorkerCost + ")" + "\n" + "[E]" + "\n" + Workers + ", " + WorkerMult;
            }
        }

        if (PlayerObject != null && HManApp != null)
        {
            float distance = Vector3.Distance(PlayerObject.position, HManApp.position);
            if (distance < 3.6 && Input.GetKeyDown(KeyCode.E) && Money >= HireManCost)
            {
                hireManagerBuy();
                MoneyText.text = "Money: $" + Money.ToString("F2");
                HireManText.text = "Hire Hiring Manager ($" + HireManCost + ")" + "\n" + "[E]" + "\n" + HireMans + ", " + HireManMult;
            }
        }

        if (PlayerObject != null && Anvil != null)
        {
            float distance = Vector3.Distance(PlayerObject.position, Anvil.position);
            if (distance < 3.6 && Input.GetKeyDown(KeyCode.E) && Money >= HMForgerCost)
            {
                HMForgeBuy();
                MoneyText.text = "Money: $" + Money.ToString("F2");
                AnvilText.text = "HM Forger ($" + HMForgerCost + ")" + "\n" + "[E]" + "\n" + HMForgers;
            }
        }
    }

    public void buyHighQualLemon()
    {
        Money = Money - HighQualLemCost;
        highQualLems++;
        HighQualLemCost = HighQualLemCost * costScaler * (highQualLems + 1);
    }

    public void hireWorker()
    {
        Money = Money - WorkerCost;
        if (Workers > 4)
        {
            WorkerMult++;
        }
        if (HireMans < 1)
        {
            Workers++;
        }
        WorkerCost = WorkerCost * costScaler;
        if (Workers < 2)
        {
            for (int i = 3; i > 0; i--)
                Worker.position += Vector3.up;
        }
    }

    public void hireManagerBuy()
    {
        Money = Money - HireManCost;
        HireMans++;
        HireManCost = HireManCost * 2;
        if(HireMans > 4){
            HireManMult++;
        }
    }

    public void HMForgeBuy()
    {
        Money = Money - HMForgerCost;
        HMForgers++;
        HMForgerCost *= 2;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
