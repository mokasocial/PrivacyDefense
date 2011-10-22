package org.aclu.freedomdefense;

public abstract class Creep {
	CreepType m_type;
	int Speed;
	int Health;
	int x, y;

	public Creep(){
		//@todo
	}
	
	public void draw(){
		// @todo
	};

	public void move(){
		// @todo
	};

	public int[][] getNextDestinationCoordinate(){
		return null;
		// @todo
	};
	
	public void die(){
		// @todo
	}

	public void update( float dt ) {
		// TODO Auto-generated method stub
		
	}

	public void getCoords() {
		// TODO Auto-generated method stub
		
	}
}
