package org.aclu.freedomdefense;

import com.badlogic.gdx.math.Vector2;

public class TowerType {

	private final String m_towerType;
	private final int m_price;
	private final Vector2 sprite_loc;

	private TowerType(String type, int price, Vector2 the_sprite_loc) {
		m_towerType = type;
		m_price = price;
		sprite_loc = the_sprite_loc;
	}

	public int getPrice() {
		return m_price;
	}

	public int getSpriteLocX() {
		return (int)sprite_loc.x;
	}
	public int getSpriteLocY() {
		return (int)sprite_loc.y;
	}
	
	public static TowerType JUDGE = new TowerType("JUDGE", 50, new Vector2( 10*16, 1*16));
	public static TowerType FIREWALL = new TowerType("FIREWALL", 100, new Vector2( 8*16, 1*16));
	public static TowerType TEACHER = new TowerType("TEACHER", 200, new Vector2( 9*16, 1*16));
	public static TowerType LAWSUIT = new TowerType("LAWSUIT", 300, new Vector2( 7*16, 1*16));
	
	public static TowerType[] Values = {
		JUDGE,
		FIREWALL,
		TEACHER,
		LAWSUIT
	};	
	
}