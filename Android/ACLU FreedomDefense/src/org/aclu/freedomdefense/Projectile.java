package org.aclu.freedomdefense;

import com.badlogic.gdx.math.Vector2;

public class Projectile
{
	private final Vector2 my_velocity;

	public Vector2 my_coords;
	
	public float damage;

	public Projectile(final Vector2 the_starting_coord, final Vector2 velocity, float damage )
	{
		my_coords = the_starting_coord;
		my_velocity = velocity;
		this.damage = damage;
	}
	
	/**
	 * Update the projectile with the specified time.
	 * 
	 * @param dt
	 *            The duration of time to update through.
	 */
	public void update( final float dt )
	{
		my_coords.x += my_velocity.x * dt;
		my_coords.y += my_velocity.y * dt;
	};
}