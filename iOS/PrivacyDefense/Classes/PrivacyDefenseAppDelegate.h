/*
 
 This iPhone application was developed over a weekend (October 21-23, 2011) at Seattle GiveCamp.
 
 Two important tools and code sources were used in the development.  The first is a commonly used game framework called cocos-2d, found at:
 
 http://www.cocos2d-iphone.org/
 
 The second important source was tutorial material and sample code found at:
 
 www.iphonegametutorials.com
 
 While code samples provided as part of a tutorial are commonly granted a full right to use in production code, the rights to use this code should be confirmed before release.
 
*/






//
//  PrivacyDefenseAppDelegate.h
//  Cocos2D Build a Tower Defense Game
//
//  Created by iPhoneGameTutorials on 4/4/11.
//  Copyright 2011 iPhoneGameTutorial.com All rights reserved.
//

#import <UIKit/UIKit.h>

@class RootViewController;

@interface PrivacyDefenseAppDelegate : NSObject <UIApplicationDelegate> {
	UIWindow			*window;
	RootViewController	*viewController;
}

@property (nonatomic, retain) UIWindow *window;

@end
