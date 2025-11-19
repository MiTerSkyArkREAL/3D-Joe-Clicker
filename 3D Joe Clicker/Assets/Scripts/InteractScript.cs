//TODO:
//Make worker move up, finish hiring system
//Hiring manager that hires workers
using UnityEngine;
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

   public float costScaler = 1.2f;


   public float Money = 0f;
   public TMP_Text MoneyText;
   public TMP_Text WallCostText;
   public TMP_Text HighQualLemonText;
   public TMP_Text WorkerText;

   public bool wallBought = false;
   public float highQualLems = 0f;
   public float HighQualLemCost = 15f;
   public float Workers = 0f;
   public float WorkerCost = 500f;



   void Start()
   {
       rb = GetComponent<Rigidbody>();
   }
   void Update()
   {
    MoneyText.text = "Money: $" + Money.ToString("F2");
        if(Workers > 0){
            Money += Workers * Time.deltaTime;
        }

       if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
       {
           rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
           isGrounded = false;
       }

       if (PlayerObject != null && Table != null)
        {
            float distance = Vector3.Distance(PlayerObject.position, Table.position);
            if(distance < 3.5 && Input.GetKeyDown(KeyCode.E)){
                Money = Money + 1 + highQualLems;
                MoneyText.text = "Money: $" + Money.ToString("F2");
            }
        }

        if (PlayerObject != null && Wall != null)
        {
            float distance = Vector3.Distance(PlayerObject.position, Wall.position);
            if(distance < 3.5 && Input.GetKeyDown(KeyCode.E) && Money >= 100 && !wallBought){
                Money = Money - 100;
                MoneyText.text = "Money: $" + Money.ToString("F2");
                jumpForce = 4f;
                WallCostText.text = "Jump Higher";
                wallBought = true;
            }
        }

        if (PlayerObject != null && Lemon != null)
        {
            float distance = Vector3.Distance(PlayerObject.position, Lemon.position);
            if(distance < 3.7 && Input.GetKeyDown(KeyCode.E) && Money >= HighQualLemCost){
                buyHighQualLemon();
                MoneyText.text = "Money: $" + Money.ToString("F2");
                HighQualLemonText.text = "Higher Quality Lemons ($" + HighQualLemCost + ")" + "\n" + "[E]" + "\n" + highQualLems;
            }
        }

        if (PlayerObject != null && JobApp != null)
        {
            float distance = Vector3.Distance(PlayerObject.position, JobApp.position);
            if(distance < 3.6 && Input.GetKeyDown(KeyCode.E) && Money >= WorkerCost){
                hireWorker();
                MoneyText.text = "Money: $" + Money.ToString("F2");
                WorkerText.text = "Hire Worker ($" + WorkerCost + ")" + "\n" + "[E]" + "\n" + Workers;
            }
        }
   }
    public void buyHighQualLemon(){
        Money = Money - HighQualLemCost;
        highQualLems++;
        HighQualLemCost = HighQualLemCost * costScaler * (highQualLems + 1);
    }

    public void hireWorker(){
        Money = Money - WorkerCost;
        Workers++;
        WorkerCost = WorkerCost * costScaler;
        if(Workers < 2){
            for(int i = 2; i > 0; i--)
                Worker.position += Vector3.up; 
            }
        }
    
   void OnCollisionEnter(Collision collision)
   {
       // Reset grounded state when colliding with the ground
       if (collision.gameObject.CompareTag("Ground"))
       {
           isGrounded = true;
       }
   }
}