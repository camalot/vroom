using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public float Acceleration { get; set; }

	public float Torque { get; set; }

	public float DriftFactorSticky { get; set; }

	public float DriftFactorSlippy { get; set; }

	public float MaxStickyVelocity { get; set; }

	public int Lap { get; set; }

	private bool LapEnter { get; set; }

	public Player()
	{
		this.Acceleration = 10f;
		this.Torque = 200f;
		this.DriftFactorSticky = 0.1f;
		this.DriftFactorSlippy = 0.999f;
		this.MaxStickyVelocity = 2.5f;
		this.Lap = 0;
		this.LapEnter = false;
	}

	private void Start()
	{
	}

	private void OnTriggerEnter2D(Collider2D c)
	{
		if (!(c.name == "lap"))
			return;
		Rigidbody2D component = this.GetComponent<Rigidbody2D>();
		if (component.velocity.y > 0.0f) {
			component.velocity = new Vector2 (0.0f, 0.0f);
			component.position = new Vector2 (component.position.x, component.position.y - 0.025f);
		} else {
			this.LapEnter = true;
		}
	}

	private void OnTriggerStay2D(Collider2D c)
	{
		if (!(c.name == "lap"))
			return;
		Rigidbody2D component = this.GetComponent<Rigidbody2D>();
		if (component.velocity.y < 0.0f)
			return;
		component.velocity = new Vector2(0.0f, 0.0f);
		component.position = new Vector2(component.position.x, component.position.y - 0.025f);
	}

	private void OnTriggerExit2D(Collider2D c)
	{
		if (!(c.name == "lap") || !this.LapEnter)
			return;
		Object.FindObjectOfType<LapManager>().UpdateLap(this.Lap++);
		this.LapEnter = false;
	}

	private void FixedUpdate()
	{
		Rigidbody2D component = this.GetComponent<Rigidbody2D>();
		float df = this.DriftFactorSticky;
		if ((double) this.RightVelocity().magnitude > (double) this.MaxStickyVelocity)
			df = this.DriftFactorSlippy;
		component.velocity = this.ForwardVelocity() + this.RightVelocity() * df;
		if (Input.GetButton("Accelerate"))
			component.AddForce((Vector2) (this.transform.right * this.Acceleration));
		else if (Input.GetButton("Brake"))
			component.AddForce((Vector2) (this.transform.right * (float) -((double) this.Acceleration / 2.0)));
		float tq = Mathf.Lerp(0.0f, this.Torque, component.velocity.magnitude / 2f);
		component.angularVelocity = Input.GetAxis("Horizontal") * tq;
	}

	private Vector2 ForwardVelocity()
	{
		return (Vector2) (this.transform.right * Vector2.Dot(this.GetComponent<Rigidbody2D>().velocity, (Vector2) this.transform.right));
	}

	private Vector2 RightVelocity()
	{
		return (Vector2) (this.transform.up * Vector2.Dot(this.GetComponent<Rigidbody2D>().velocity, (Vector2) this.transform.up));
	}
}
