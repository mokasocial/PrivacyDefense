package org.aclu.freedomdefense.animator;

import org.aclu.freedomdefense.Animator;

import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.graphics.Texture;
import com.badlogic.gdx.graphics.g2d.Animation;
import com.badlogic.gdx.graphics.g2d.SpriteBatch;
import com.badlogic.gdx.graphics.g2d.TextureRegion;

public class CriminalCreepAnimator extends Animator {

	private static final int FRAME_COLS = 3;
	private static final int FRAME_ROWS = 1;

	@Override
	public void create() {
		walkSheet = new Texture(Gdx.files.internal("criminal_creep_animation_sheet.png"));
		TextureRegion[][] tmp = TextureRegion.split(walkSheet, walkSheet.getWidth() / FRAME_COLS, walkSheet.getHeight() / FRAME_ROWS);
		walkFrames = new TextureRegion[FRAME_COLS * FRAME_ROWS];
		int index = 0;
		for (int i = 0; i < FRAME_ROWS; i++) {
			for (int j = 0; j < FRAME_COLS; j++) {
				walkFrames[index++] = tmp[i][j];
			}
		}
		walkAnimation = new Animation(0.025f, walkFrames);
		spriteBatch = new SpriteBatch();
		stateTime = 0f;
	}
}
