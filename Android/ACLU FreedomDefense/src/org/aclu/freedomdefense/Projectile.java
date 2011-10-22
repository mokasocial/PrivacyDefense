package org.aclu.freedomdefense;

import com.badlogic.gdx.math.Vector2;

public abstract class Projectile {
	
	public enum ProjectileType {
		/**
		 * Standard Projectile with 0.5f velocity.
		 */
		STANDARD(0.5f),
		/**
		 * Other Projectile with 1.0f velocity.
		 */
		OTHER(1.0f);
		
		private float velocity;
		private ProjectileType(float the_velocity) {
			velocity = the_velocity;
		}
		
		public float getVelocity() {
			return velocity;
		}
	}
	
	private final ProjectileType my_type;
	
	private boolean my_alive_state;
	
	Vector2 my_coords;
	Vector2 my_direction;
	
	public Projectile(final Vector2 the_starting_coord,
					  final Vector2 the_firing_direction,
					  final ProjectileType the_type) {
		if (the_starting_coord == null) {
			throw new IllegalArgumentException("non-null starting coordinates required");
		}
		
		if (the_firing_direction == null ) {
			throw new IllegalArgumentException("non-null firing direction required");
		}

		my_coords = the_starting_coord;
		my_type = the_type;
		my_direction = the_firing_direction;
		my_alive_state = true;
		
	}

	/**
	 * Retireve the location of the projectile.
	 * 
	 * Element 0: x coordinate
	 * Element 1: y coordinate
	 * @return int array with coordinates.
	 */
	public Vector2 getCoordinates() {
		return my_coords;
	}
	
	private void move(final float dt){

		// Trying to remember my math.
		my_coords.x += (my_type.getVelocity() * my_direction.x) * dt;  
	    my_coords.y += (my_type.getVelocity() * my_direction.y) * dt;
		
	}
	
	/**
	 * Set the projectile to dead.
	 */
	public void die(){
		my_alive_state = false;
	}

	/**
	 * Update the projectile with the specified time.
	 * @param dt The duration of time to update through.
	 */
	public void update(final float dt) {
		
		if (my_alive_state) {
			move(dt);
		}
		
	};
}