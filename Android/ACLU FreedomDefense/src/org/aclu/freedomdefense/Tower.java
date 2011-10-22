package org.aclu.freedomdefense;

public class Tower {
	int m_x;
	int m_y;
	private final TowerType m_type;
	public int m_speed;
	public float m_timeToFire;
	public float radius;

	public Tower(TowerType type, int x, int y) 
	{
		m_type = type;
		m_x = x;
		m_y = y;
		
		if (type.equals(TowerType.JUDGE))
		{
			m_speed = 5;
			radius = 20;
		}
		else if (type.equals(TowerType.FIREWALL))
		{
			m_speed = 6;
			radius = 25;
		}
		else if (type.equals(TowerType.TEACHER))
		{
			m_speed = 7;
			radius = 30;
		}
		else 
		{
			m_speed = 8;
			radius = 35;
		}
		
		m_timeToFire = m_speed;
	}

	public void move(int x, int y) 
	{
		m_x = x;
		m_y = y;
	}
	
	public int getIconNum() 
	{
		if (m_type.equals(TowerType.JUDGE))
			return 0;
		else if (m_type.equals(TowerType.FIREWALL))
			return 2;
		else if (m_type.equals(TowerType.TEACHER))
			return 1;
		else 
			return 3;
	}
	
	public void update(float deltaTime) 
	{
		m_timeToFire -= deltaTime;
		
		// We're ready and there are creeps!
		if( m_timeToFire < 0 && Game.instance.creeps.size() > 0 ) 
		{
			// Find the enemy closest to the goal
			float closestDist2 = 999999.0f;
			int closestIndex = 0;
			
			for( int i = 0; i < Game.instance.creeps.size(); ++i )
			{
				float dist2 = (float) (Math.pow( Game.instance.creeps.get(i).x - Game.instance.endingX, 2) + Math.pow( Game.instance.creeps.get(i).y, 2));
				
				if( dist2 < closestDist2 )
					closestIndex = 0;
			}
			
			// Reset the cooldown for next frame
			m_timeToFire = m_speed;
		}
	}
}
