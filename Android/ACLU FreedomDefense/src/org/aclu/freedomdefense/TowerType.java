package org.aclu.freedomdefense;

public class TowerType {

	private final String m_towerType;

	private TowerType(String type) {
		m_towerType = type;
	}

	public static TowerType JUDGE = new TowerType("JUDGE");
	public static TowerType FIREWALL = new TowerType("FIREWALL");
	public static TowerType TEACHER = new TowerType("TEACHER");
	public static TowerType LAWSUIT = new TowerType("LAWSUIT");
	
	public static TowerType[] Values = {
		JUDGE,
		FIREWALL,
		TEACHER,
		LAWSUIT
	};	
}