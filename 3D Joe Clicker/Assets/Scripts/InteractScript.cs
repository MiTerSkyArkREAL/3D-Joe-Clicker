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

   public float Money = 0f;
   public TMP_Text MoneyText;
   public TMP_Text WallCostText;


   void Start()
   {
       rb = GetComponent<Rigidbody>();
   }
   void Update()
   {
       if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
       {
           rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
           isGrounded = false;
       }

       if (PlayerObject != null && Table != null)
        {
            float distance = Vector3.Distance(PlayerObject.position, Table.position);
            if(distance < 3.5 && Input.GetKeyDown(KeyCode.E)){
                Money++;
                MoneyText.text = "Money: $" + Money.ToString();
            }
        }

        if (PlayerObject != null && Wall != null)
        {
            float distance = Vector3.Distance(PlayerObject.position, Wall.position);
            if(distance < 3.5 && Input.GetKeyDown(KeyCode.E) && Money >= 100){
                Money = Money - 100;
                MoneyText.text = "Money: $" + Money.ToString();
                jumpForce = 4f;
                WallCostText.text = "Jump Higher";
            }
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