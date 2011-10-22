package org.aclu.freedomdefense;

public class Tower {
	int m_x;
	int m_y;
	private final TowerType m_type;
	private int m_speed;
	private float m_timeToFire;

	public Tower(TowerType type, int x, int y) {
		m_type = type;
		m_x = x;
		m_y = y;
		
		if (type.equals(TowerType.JUDGE))
			m_speed = 5;
		else if (type.equals(TowerType.FIREWALL))
			m_speed = 6;
		else if (type.equals(TowerType.TEACHER))
			m_speed = 7;
		else 
			m_speed = 8;
		
		m_timeToFire = m_speed;
	}

	public void move(int x, int y) {
		m_x = x;
		m_y = y;
	}
	
	//	public void update(Projectile[] projectiles, float deltaTime) {
	public void update(float deltaTime) {
		float k = m_timeToFire - deltaTime;
		if (k < 0) {
			
			/* Shoot */
			//Projectile p = new Projectile(m_type);
			//projectiles.add(p);
			
			m_timeToFire = m_speed;
		} else
			m_timeToFire = k;
	}
}
