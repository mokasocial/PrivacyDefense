//
//  Creep.h
//  Cocos2D Build a Tower Defense Game
//
//  Created by iPhoneGameTutorials on 4/4/11.
//  Copyright 2011 iPhoneGameTutorial.com All rights reserved.
//

#import "cocos2d.h"
#import "Creep.h"
#import "SimpleAudioEngine.h"
#import "Projectile.h"
#import "DataModel.h"

@interface Tower : CCSprite {
	int _range;
	
	Creep * _target;
	
	CCSprite * selSpriteRange;
	
	NSMutableArray *_projectiles;
	CCSprite *_nextProjectile;
}

@property (nonatomic, assign) int range;

@property (nonatomic, retain) CCSprite * nextProjectile;
@property (nonatomic, retain) Creep * target;

- (Creep *)getClosestTarget;

@end

@interface MachineGunTower : Tower {

}

+ (id)tower;

- (void)setClosestTarget:(Creep *)closestTarget;
- (void)towerLogic:(ccTime)dt;
- (void)creepMoveFinished:(id)sender;
- (void)finishFiring;

@end
