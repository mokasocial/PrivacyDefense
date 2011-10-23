package org.aclu.freedomdefense;

import java.util.Random;

public class CreepType {

	private static final Random rand = new Random();
	private final String m_type;

	private CreepType(String type) {
		m_type = type;
	}

	public static CreepType CRIMINAL = new CreepType("CRIMINAL");
	public static CreepType BOSS = new CreepType("BOSS");
	public static CreepType GLOBAL_CORP = new CreepType("GLOBAL_CORP");
	public static CreepType GOVERNMENT = new CreepType("GOVERNMENT");
	public static CreepType PETTY = new CreepType("PETTY");
	public static CreepType SEARCHER = new CreepType("SEARCHER");
	
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