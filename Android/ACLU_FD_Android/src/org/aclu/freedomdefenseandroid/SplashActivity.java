package org.aclu.freedomdefenseandroid;
import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;

public class SplashActivity extends Activity 
{	
	protected boolean _active = true;
	protected int _splashTime = 2000;
	
    /** Called when the activity is first created. */
    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        
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
					if (isFirstTime(MainActivity.TOUR_WELCOME)) {
						startActivity(new Intent(mContext, TourWelcomeActivity.class));
					} else {
						
						/**
						 * @todo We'll eventually want to be calling a login activity or something else...
						 */
						startActivity(new Intent(mContext, MainActivity.class));
					}
					finish();
				}
			}
		};
		splashTread.start();
    }
}