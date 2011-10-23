package org.aclu.freedomdefense;

import java.util.Random;

import com.badlogic.gdx.math.Vector2;

public class Tower 
{
	int m_x;
	int m_y;
	final TowerType m_type;
	public float m_speed;
	public float m_timeToFire;
	public float radius;
	public float bullet_velocity;
	public float bullet_damage;
	private Random rando;
	public boolean selected = false;
	
	public int level = 1;

	public Tower(TowerType type, int x, int y) 
	{
		m_type = type;
		m_x = x;
		m_y = y;
		
		bullet_velocity = 7.0f;
		
		if (type.equals(TowerType.JUDGE))
		{
			m_speed = 0.3f;
			radius = 5;
			bullet_damage = 10;
		}
		else if (type.equals(TowerType.FIREWALL))
		{
			m_speed = 0.25f;
			radius = 6;
			bullet_damage = 11;
		}
		else if (type.equals(TowerType.TEACHER))
		{
			m_speed = 0.15f;
			radius = 7;
			bullet_damage = 12;
		}
		else 
		{
			m_speed = 0.1f;
			radius = 5;
			bullet_damage = 13.0f;
		}
		
		m_timeToFire = m_speed;
		
		rando = new Random();
	}

	public void move(int x, int y) 
	{
		m_x = x;
		m_y = y;
	}
	
	public int getIconNum() 
	{
		if (m_type.equals(TowerType.JUDGE))
			return 10;
		else if (m_type.equals(TowerType.FIREWALL))
			return 7;
		else if (m_type.equals(TowerType.TEACHER))
			return 9;
		else 
			return 8;
	}
	
	public void update(float deltaTime) 
	{
		m_timeToFire -= deltaTime;
		
		// We're ready and there are creeps!
		if( m_timeToFire < 0 && Game.instance.creeps.size() > 0 ) 
		{
			// Find the enemy closest to the goal
			float closestDistanceToGoal = 999999.0f;
			float closestDistanceToTower = 99999.0f;
			int closestIndex = 0;
			
			for( int i = 0; i < Game.instance.creeps.size(); ++i )
			{
				if( !Game.instance.creeps.get(i).active )
					continue;
				
				float distanceToTower = (float)Math.sqrt( Math.pow( m_x - Game.instance.creeps.get(i).x, 2 ) + Math.pow( m_y - Game.instance.creeps.get(i).y, 2) );
				
				if( distanceToTower < radius )
				{
					float distanceToGoal = (float) (Math.pow( Game.instance.endingX - Game.instance.creeps.get(i).x, 2) + Math.pow( Game.instance.endingY - Game.instance.creeps.get(i).y, 2));
				
					if( distanceToGoal < closestDistanceToGoal )
					{
						closestIndex = i;
						closestDistanceToGoal = distanceToGoal;
						closestDistanceToTower = distanceToTower;
					}
				}
			}
			
			// Fire something @ Game.instance.creeps.get(index)!
			if( closestDistanceToGoal < 9999.0f ) 
			{
			 	//int xDirection =  Game.instance.creeps.get(closestIndex).x - m_x;
			 	//int yDirection =  Game.instance.creeps.get(closestIndex).y - m_y;
			 	//Vector2 directionVector = new Vector2(xDirection, yDirection);
			 	//Vector2 scaledDirectionVector = directionVector.nor().mul(bullet_velocity);
			 	
			 	for( int i = 0; i < Game.maxProjectiles; ++i )
			 	{
			 		if( !Game.instance.projectiles[i].active )
			 		{
			 			Game.instance.projectiles[i].active = true;
			 			Game.instance.projectiles[i].damage = bullet_damage;
			 			Game.instance.projectiles[i].my_coords.x = m_x;
			 			Game.instance.projectiles[i].my_coords.y = m_y;
			 			Game.instance.projectiles[i].my_velocity.x = ( ( Game.instance.creeps.get(closestIndex).x - m_x ) / closestDistanceToTower ) * bullet_velocity;
			 			Game.instance.projectiles[i].my_velocity.y = ( ( Game.instance.creeps.get(closestIndex).y - m_y ) / closestDistanceToTower ) * bullet_velocity;
			 			break;
			 		}
			 	}
			 	
			 	// Reset the cooldown for next frame
			 	m_timeToFire = m_speed;
			 }
			else
			{
				// Needs time to wake up, heh (so we don't have too many towers trying to shoot every frame)
				m_timeToFire = 0.5f + rando.nextFloat();
			}
		}
	}
}
