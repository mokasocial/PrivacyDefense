package org.aclu.freedomdefense;

public abstract class Projectile {
	
	public enum ProjectileType {
		STANDARD(0.5f),
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
	
	private float my_x;
	private float my_y;
	
	private float[] my_direction;
	
	private boolean my_alive_state;
	
	
	public Projectile(final int[] the_starting_coord, final float[] the_firing_direction) 
	{
		if (the_starting_coord == null) {
			throw new IllegalArgumentException("non-null starting coordinates required");
		}
		if (the_firing_direction == null ) {
			throw new IllegalArgumentException("non-null firing direction required");
		}
		if (the_firing_direction.length < 2) {
			throw new IllegalArgumentException("firing direction requires an x and y component");
		}
		my_x = the_starting_coord[0];
		my_y = the_starting_coord[1];
		my_type = ProjectileType.STANDARD;
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
	public float[] getCoordinates() {
		return new float[] { my_x, my_y };
	}
	
	private void move(final float dt){

		// Trying to remember my math.
		my_x += (my_type.getVelocity() * my_direction[0]) * dt;  
	    my_y += (my_type.getVelocity() * my_direction[1]) * dt;
		
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