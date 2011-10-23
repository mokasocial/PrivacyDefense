package org.aclu.freedomdefenseandroid;

import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.os.Bundle;

public class SplashActivity extends Activity {
	protected boolean _active = true;
	protected int _splashTime = 2000;
	private Context mContext;
	
	/** Called when the activity is first created. */
	@Override
	public void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);

		mContext = this;
		
		// splashy!
		setContentView(R.layout.splash);

		// thread for displaying the SplashScreen
		Thread splashTread = new Thread() {
			@Override
			public void run() {
				try {
					int waited = 0;
					while (_active && (waited < _splashTime)) {
						sleep(100);
						if (_active) {
							waited += 100;
						}
					}
				} catch (InterruptedException e) {
					// do nothing
				} finally {
					startActivity(new Intent(mContext, AndroidGame.class));
					finish();
				}
			}
		};
		splashTread.start();
	}
}