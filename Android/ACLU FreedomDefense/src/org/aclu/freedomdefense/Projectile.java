package org.aclu.freedomdefense;

import com.badlogic.gdx.math.Vector2;

public abstract class Projectile {
	
	private boolean my_alive_state;
	
	private final Vector2 my_direction;

	private final float my_velocity;

	private Vector2 my_coords;
	
	public Projectile(final Vector2 the_starting_coord,
					  final Vector2 the_firing_direction,
					  final float the_velocity) {
		if (the_starting_coord == null) {
			throw new IllegalArgumentException("non-null starting coordinates required");
		}
		
		if (the_firing_direction == null ) {
			throw new IllegalArgumentException("non-null firing direction required");
		}
		my_velocity = the_velocity;
		my_coords = the_starting_coord;
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
		my_coords.x += (my_velocity * my_direction.x) * dt;  
	    my_coords.y += (my_velocity * my_direction.y) * dt;
		
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