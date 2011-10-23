package org.aclu.freedomdefense;

import com.badlogic.gdx.math.Vector2;

public class Tower 
{
	int m_x;
	int m_y;
	private final TowerType m_type;
	public float m_speed;
	public float m_timeToFire;
	public float radius;
	public float bullet_velocity;
	public float bullet_damage;

	public Tower(TowerType type, int x, int y) 
	{
		m_type = type;
		m_x = x;
		m_y = y;
		
		bullet_velocity = 7.0f;
		
		if (type.equals(TowerType.JUDGE))
		{
			m_speed = 0.25f;
			radius = 5;
			bullet_damage = 20;
		}
		else if (type.equals(TowerType.FIREWALL))
		{
			m_speed = 0.5f;
			radius = 6;
			bullet_damage = 15;
		}
		else if (type.equals(TowerType.TEACHER))
		{
			m_speed = 0.75f;
			radius = 7;
			bullet_damage = 10;
		}
		else 
		{
			m_speed = 1.0f;
			radius = 8;
			bullet_damage = 5;
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
			float closestDistanceToGoal = 999999.0f;
			int closestIndex = 0;
			
			for( int i = 0; i < Game.instance.creeps.size(); ++i )
			{
				float distanceToTower = (float)Math.sqrt( Math.pow( m_x - Game.instance.creeps.get(i).x, 2 ) + Math.pow( m_y - Game.instance.creeps.get(i).y, 2) );
				
				if( distanceToTower < radius )
				{
					float distanceToGoal = (float) (Math.pow( Game.instance.endingX - Game.instance.creeps.get(i).x, 2) + Math.pow( Game.instance.endingY - Game.instance.creeps.get(i).y, 2));
				
					if( distanceToGoal < closestDistanceToGoal )
					{
						closestIndex = i;
						closestDistanceToGoal = distanceToGoal;
					}
				}
			}
			
			// Fire something @ Game.instance.creeps.get(index)!
			if( closestDistanceToGoal < 9999.0f ) 
			{
			 	int xDirection =  Game.instance.creeps.get(closestIndex).x - m_x;
			 	int yDirection =  Game.instance.creeps.get(closestIndex).y - m_y;
			 	Vector2 directionVector = new Vector2(xDirection, yDirection);
			 	Vector2 scaledDirectionVector = directionVector.nor().mul(bullet_velocity);
			 	Projectile projectile = new Projectile(new Vector2(m_x,m_y), scaledDirectionVector, bullet_damage);
			 	Game.instance.projectiles.add(projectile);
			 	
			 	// Reset the cooldown for next frame
			 	m_timeToFire = m_speed;
			 }
		}
	}
}
