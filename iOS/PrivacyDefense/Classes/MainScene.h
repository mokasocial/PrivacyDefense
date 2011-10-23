//
//  TutorialLayer.h
//  Cocos2D Build a Tower Defense Game
//
//  Created by iPhoneGameTutorials on 4/4/11.
//  Copyright 2011 iPhoneGameTutorial.com All rights reserved.
//


// When you import this file, you import all the cocos2d classes
#import "cocos2d.h"

#import "Creep.h"
#import "Projectile.h"
#import "Tower.h"
#import "WayPoint.h"
#import "Wave.h"

#import "GameHUD.h"

// Layer
@interface MainScene : CCLayer
{
    CCTMXTiledMap *_tileMap;
    CCTMXLayer *_background;	
	
	int _currentLevel;
	
	GameHUD * gameHUD;
}

@property (nonatomic, retain) CCTMXTiledMap *tileMap;
@property (nonatomic, retain) CCTMXLayer *background;

@property (nonatomic, assign) int currentLevel;

+ (id) scene;
- (void)addWaypoint;
- (void)addWaves;
- (void)addTower: (CGPoint)pos;
- (BOOL) canBuildOnTilePosition:(CGPoint) pos;

@end
