package org.aclu.freedomdefense;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.Input;
import com.badlogic.gdx.Input.Buttons;
import com.badlogic.gdx.InputProcessor;

public class GameInputProcessor implements InputProcessor {

	public static final int DEFAULT_WIN_PAUSE_KEY = Input.Keys.SPACE;
	public static final int DEFAULT_DRD_PAUSE_KEY = Input.Keys.BACK;
	
	public GameInputProcessor() {
		Gdx.input.setInputProcessor(this);
		// other constructor stuff here...
	}

	@Override
	public boolean keyDown(int keycode) {
		return false;
	}

	@Override
	public boolean keyTyped(char character) {
		//Game.instance.debugtext = "key typed: '" + character + "'";
		return false;
	}

	@Override
	public boolean keyUp(int keycode) {
		// Game.instance.debugtext = "key up: " + keycode;
		if (keycode == DEFAULT_WIN_PAUSE_KEY || keycode == DEFAULT_DRD_PAUSE_KEY) {
			Game.instance.pause();
		}
		return false;
	}

	@Override
	public boolean touchDown(int x, int y, int pointer, int button) {
		// we only want single / left clicks
		if (pointer != 0 || button != Buttons.LEFT) {
			return false;
		}
		Game.instance.debugtext = "touch down: " + x + ", " + y + ", button: " + getButtonString(button);

		// Check for touch down on the pause button.
		if (Game.instance.runningDrd &&
				x >=  Game.DRD_PAUSE_RECT.x && x <= Game.DRD_PAUSE_RECT.x + Game.DRD_PAUSE_RECT.width &&
				(Game.screenHeight - y) >=  Game.DRD_PAUSE_RECT.y && (Game.screenHeight - y) <= Game.DRD_PAUSE_RECT.y + Game.DRD_PAUSE_RECT.height) {
			Game.instance.debugtext = "touch down on pause rect";
			Game.instance.cursorState = null;
			Game.instance.cursorLocX = x;
			Game.instance.cursorLocY = y;
			Game.instance.pause();
			return false;
		}
		
		for (TowerType towerType : Game.instance.free_towers) {
			if (x <= Game.instance.uiPanelWidth) {
				if (y >= (32) && y <= (48)) {
					Game.instance.debugtext = "touch down on tower 1";
					Game.instance.cursorState = TowerType.FIREWALL;
					Game.instance.cursorLocX = x;
					Game.instance.cursorLocY = y;
				} else if (y >= (80) && y <= (96)) {
					Game.instance.debugtext = "touch down on tower 2";
					Game.instance.cursorState = TowerType.JUDGE;
					Game.instance.cursorLocX = x;
					Game.instance.cursorLocY = y;
				} else if (y >= (128) && y <= (144)) {
					Game.instance.debugtext = "touch down on tower 3";
					Game.instance.cursorState = TowerType.LAWSUIT;
					Game.instance.cursorLocX = x;
					Game.instance.cursorLocY = y;
				} else if (y >= (176) && y <= (192)) {
					Game.instance.debugtext = "touch down on tower 4";
					Game.instance.cursorState = TowerType.TEACHER;
					Game.instance.cursorLocX = x;
					Game.instance.cursorLocY = y;
				}
			}
		}
		return false;
	}

	@Override
	public boolean touchDragged(int x, int y, int pointer) {
		Game.instance.debugtext = "touch dragged: " + x + ", " + y + ", pointer: " + pointer;
		return false;
	}

	@Override
	public boolean touchUp(int x, int y, int pointer, int button) {
		
		Game.instance.cursorLocX = 0;
		Game.instance.cursorLocY = 0;
		Game.instance.cursorState = null;
		
		if (Buttons.LEFT != button || pointer != 0) {
			return false;
		}
		// Game.instance.debugtext = "touch up: " + x + ", " + y + ", button: "
		// + getButtonString(button);
		return false;
	}

	@Override
	public boolean touchMoved(int x, int y) {
		Game.instance.debugtext = "touch moved: " + x + ", " + y;
		return false;
	}

	@Override
	public boolean scrolled(int amount) {
		// Game.instance.debugtext = "scrolled: " + amount;
		return false;
	}

	private String getButtonString(int button) {
		if (button == Buttons.LEFT)
			return "left";
		if (button == Buttons.RIGHT)
			return "right";
		if (button == Buttons.MIDDLE)
			return "middle";
		return "left";
	}
}
