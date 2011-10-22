package org.aclu.freedomdefenseandroid;

import org.aclu.freedomdefense.Game;

import com.badlogic.gdx.backends.android.AndroidApplication;

public class AndroidGame extends AndroidApplication {
	@Override
	public void onCreate(android.os.Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		initialize(new Game(), false);
	}
}
