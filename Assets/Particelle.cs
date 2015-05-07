using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class Particelle : MonoBehaviour {

	ParticleSystem m_System;
	ParticleSystem.Particle[] m_Particles;
	public Transform target;
	public float EseguiDopoSec = 0f;
	public float m_Drift = 0.01f;
	public float VelocitaSpostamento = 1f;
	public float senoMax = 10f;
	public float RiduciOscillio = 250;
	public float TempoOscillio = 2;
	public bool OscillioInX = true;
	public bool OscillioInY = true;
	public bool OscillioInZ = true;
	private float OscillioX;
	private float OscillioY;
	private float OscillioZ;
	private float[] m_sinNumber;

	void LateUpdate()
	{
		InitializeIfNeeded();

		//GetParticles is allocation free because we reuse the m_Particles buffer between updates
		int numParticlesAlive = m_System.GetParticles(m_Particles);

		//int y = 1;

		//Change only the paricles that are alive
		for(int i = 0; i < numParticlesAlive; i++)
		{

			/*if(i==0)
			{
				if(m_sinNumber[i] > senoMax)
				{
					y = -1;
				}
				else if(m_sinNumber[i] < -senoMax)
				{
					y = 1;
				}
				Debug.Log ( "Seno prima particella: " + m_sinNumber[i] + " e y: " + y + " e senoMax: " + senoMax);
			}*/

			if(m_Particles[i].lifetime < m_Particles[i].startLifetime - EseguiDopoSec && target)
			{
				//fa alzare le particelle esponenzialmente
				m_Particles[i].velocity += Vector3.up * m_Drift;

				//sposta le particelle in un punto //OLD //quello successivo va bene, ma è da rivedere
				m_Particles[i].position = Vector3.Lerp(m_Particles[i].position, target.transform.position, Time.deltaTime * VelocitaSpostamento);

				//fa oscillare le particelle //forse è leggermente buggato
				//sposta le particelle in un punto e le fa oscillare in un unico script
				/*m_sinNumber[i] += Time.deltaTime * TempoOscillio;
				float sinTemp = Mathf.Sin(m_sinNumber[i])/RiduciOscillio;
				if (OscillioInX){OscillioX = sinTemp;} else{OscillioX = 0f;} //La variabile oscillio è da rivedere, ma con il secondo script integrato per fare il vortice andrebbero comunque disattivati
				if (OscillioInY){OscillioY = sinTemp;} else{OscillioY = 0f;}
				if (OscillioInZ){OscillioZ = sinTemp;} else{OscillioZ = 0f;}
				Vector3 posizioneP = new Vector3(target.transform.position.x + OscillioX, target.transform.position.y + OscillioY, target.transform.position.z + OscillioZ);
				m_Particles[i].position = Vector3.Lerp(m_Particles[i].position, posizioneP, Time.deltaTime * VelocitaSpostamento);*/


				//Vector3 differenza = target.position - m_Particles[i].position;
				//m_Particles[i]
			}
		}

		//apply the particle changes to the particle system
		m_System.SetParticles(m_Particles, numParticlesAlive);

		//OLD STUFF
		//var particleDirection :Vector3 = -rBallControls.GetGravDir();
		//lavaSmoke.rotation = Quaternion.LookRotation(Vector3.forward,particleDirection); 
	}

	void InitializeIfNeeded()
	{
		if (m_System == null)
		{
			m_System = GetComponent<ParticleSystem>();
		}

		if(m_Particles == null || m_Particles.Length < m_System.maxParticles)
		{
			m_Particles = new ParticleSystem.Particle[m_System.maxParticles];
			m_sinNumber = new float[m_System.maxParticles];
		}
	}

}


/*
var target : Transform; var tempo : float =0;

function Update(){ tempo+=Time.deltaTime;

     transform.Translate(Vector3.forward*Time.deltaTime*speed);
     
     if(rotacao>=-40 && rotacao<=40){
     
     var relativePoint = transform.InverseTransformPoint(target.transform.position);
         
     if (relativePoint.x < 0.0){
         transform.Rotate(0, rotacaospeed*-1 , 0);
         rotacao-=rotacaospeed;
     }else if (relativePoint.x > 0.0) {
         transform.Rotate(0, rotacaospeed , 0);
         rotacao+=rotacaospeed;
     }else{
         transform.Rotate(0,0,0);
     }
     }
 
      if(tempo>=2){
            Destroy(gameObject);
       }
*/
