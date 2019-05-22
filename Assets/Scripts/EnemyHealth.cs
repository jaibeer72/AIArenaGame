using UnityEngine;
using UnityEngine.UI; 
public class EnemyHealth : MonoBehaviour
{

    public int health;
    public int currentHealth;
    public Canvas damageTextPrefab;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = health;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void TakeDamage(bool canTakeDamag, int damage)
    {
        if (canTakeDamag)
        {


            if (currentHealth > 0)
            {
                Canvas damageText = Instantiate(damageTextPrefab, this.transform.position + new Vector3(0, 2.0f, 0), this.transform.rotation, transform) as Canvas;
                damageText.transform.LookAt(-Camera.main.transform.position);
                //damageText.transform.Translate(Vector3.up * Time.deltaTime * 10f);
                damageText.GetComponentInChildren<Text>().text = damage.ToString();
                Destroy(damageText.gameObject, 1);
                float amout = 1f - ((float)currentHealth / (float)health);
                //Debug.Log(amout);
                currentHealth -= damage;

            }
            else
            {
                if (gameObject.GetComponent<TankAIController>() != null)
                {
                    gameObject.GetComponent<TankAIController>().tankState = AIStates.dead;
                    Destroy(gameObject, 10); 
                }
                if (gameObject.GetComponent<TrollAIController>() != null)
                {
                    gameObject.GetComponent<TrollAIController>().trollState = AIStates.dead;
                    Destroy(gameObject, 6);
                }
            }
        }

    }
}
