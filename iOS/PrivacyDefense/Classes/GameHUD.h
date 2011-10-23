//
//  GameHUD.h
//  Cocos2D Build a Tower Defense Game
//
//  Created by iPhoneGameTutorials on 4/4/11.
//  Copyright 2011 iPhoneGameTutorial.com All rights reserved.
//

#import "cocos2d.h"


@interface GameHUD : CCLayer {
	CCSprite * background;
	
	CCSprite * selSpriteRange;
    CCSprite * selSprite;
    NSMutableArray * movableSprites;
}

+ (GameHUD *)sharedHUD;

@end
