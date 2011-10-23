package org.aclu.freedomdefense;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.Input.Buttons;
import com.badlogic.gdx.InputProcessor;

public class GameInputProcessor implements InputProcessor {

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
		// Game.instance.debugtext = "key typed: '" + character + "'";
		return false;
	}

	@Override
	public boolean keyUp(int keycode) {
		// Game.instance.debugtext = "key up: " + keycode;
		return false;
	}

	@Override
	public boolean touchDown(int x, int y, int pointer, int button) {
		// we only want single / left clicks
		if (pointer != 0 || button != Buttons.LEFT) {
			return false;
		}
		Game.instance.debugtext = "touch down: " + x + ", " + y + ", button: " + getButtonString(button);
		return false;
	}

	@Override
	public boolean touchDragged(int x, int y, int pointer) {
		Game.instance.debugtext = "touch dragged: " + x + ", " + y + ", pointer: " + pointer;
		return false;
	}

	@Override
	public boolean touchUp(int x, int y, int pointer, int button) {
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
