//TODO:
//Parkour course; prize triples current amount of money
//Upgrade that lowers cost scaler
//Outside the walls are lemon monsters. The player can buy weapons/gear to kill them. Each kill doubles money and gives a high quality lemon
//Secret wall opens only on tuesdays. A secret wall within the secret room only opens on mondays.
//Add saving when exiting
using UnityEngine;
using System;
using TMPro;

[RequireComponent(typeof(Rigidbody))]
public class InteractionScript : MonoBehaviour
{
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

    public float costScaler = 1.2f;

    public float Money = 0f;
    public TMP_Text MoneyText;
    public TMP_Text WallCostText;
    public TMP_Text HighQualLemonText;
    public TMP_Text WorkerText;
    public TMP_Text HireManText;

    public bool wallBought = false;
    public float highQualLems = 0f;
    public float HighQualLemCost = 15f;
    public float Workers = 0f;
    public float WorkerCost = 500f;
    public float WorkerMult = 1f;
    public float HireMans = 0f;
    public float HireManCost = 6000;

    DayOfWeek today = DateTime.Now.DayOfWeek;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (DateTime.Now.DayOfWeek == DayOfWeek.Friday)
        {
            for (int i = 3; i > 0; i--)
                SecretWall.position += Vector3.down;
        }
    }

    void Update()
    {
        MoneyText.text = "Money: $" + Money.ToString("F2");
        WorkerText.text = "Hire Worker ($" + WorkerCost + ")" + "\n" + "[E]" + "\n" + Workers + ", " + WorkerMult;

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

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
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
                HireManText.text = "Hire Hiring Manager ($" + HireManCost + ")" + "\n" + "[E]" + "\n" + HireMans;
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
        HireManCost = HireManCost * costScaler;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
