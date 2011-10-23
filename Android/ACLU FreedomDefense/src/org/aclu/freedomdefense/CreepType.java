package org.aclu.freedomdefense;

import java.util.Random;

public class CreepType {

	private static final Random rand = new Random();
	private final String m_type;
	public int m_worth;

	private CreepType(String type, int worth) {
		m_type = type;
		m_worth = worth;
	}

	public int getWorth() {
		return m_worth;
	}
	
	public static CreepType CRIMINAL = new CreepType("CRIMINAL", 2);
	public static CreepType BOSS = new CreepType("BOSS", 3);
	public static CreepType GLOBAL_CORP = new CreepType("GLOBAL_CORP", 4);
	public static CreepType GOVERNMENT = new CreepType("GOVERNMENT", 4);
	public static CreepType PETTY = new CreepType("PETTY",5);
	public static CreepType SEARCHER = new CreepType("SEARCHER", 5);
	
	public static void seedWithWave(final long wave) {
		rand.setSeed(wave);
	}
	
	public static CreepType getRandomCreepType() {
		final int randomIndex = rand.nextInt(6);
		return Values[randomIndex];
	}

	public static CreepType[] Values = {
		CRIMINAL,
		BOSS,
		GLOBAL_CORP,
		GOVERNMENT,
		PETTY,
		SEARCHER
	};	
}