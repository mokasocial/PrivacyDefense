//
//  Creep.m
//  Cocos2D Build a Tower Defense Game
//
//  Created by iPhoneGameTutorials on 4/4/11.
//  Copyright 2011 iPhoneGameTutorial.com All rights reserved.
//

#import "Tower.h"

@implementation Tower

@synthesize range = _range;

@synthesize target = _target;
@synthesize nextProjectile = _nextProjectile;

- (Creep *)getClosestTarget {
	Creep *closestCreep = nil;
	double maxDistant = 99999;
	
	DataModel *m = [DataModel getModel];
	
	for (CCSprite *target in m._targets) {	
		Creep *creep = (Creep *)target;
		double curDistance = ccpDistance(self.position, creep.position);
		
		if (curDistance < maxDistant) {
			closestCreep = creep;
			maxDistant = curDistance;
		}
		
	}
	
	if (maxDistant < self.range)
		return closestCreep;
	
	return nil;
}

@end

@implementation MachineGunTower

+ (id)tower {
	
    MachineGunTower *tower = nil;
    if ((tower = [[[super alloc] initWithFile:@"MachineGunTurret.png"] autorelease])) {
		tower.range = 200;
		
		tower.target = nil;
		
		[tower schedule:@selector(towerLogic:) interval:0.2];
		
    }
	
    return tower;
    
}

-(id) init
{
	if ((self=[super init]) ) {
		//[[CCTouchDispatcher sharedDispatcher] addTargetedDelegate:self priority:0 swallowsTouches:YES];
	}
	return self;
}


-(void)setClosestTarget:(Creep *)closestTarget {
	self.target = closestTarget;
}

-(void)towerLogic:(ccTime)dt {
	
	self.target = [self getClosestTarget];
	
	if (self.target != nil) {
		
		//rotate the tower to face the nearest creep
		CGPoint shootVector = ccpSub(self.target.position, self.position);
		CGFloat shootAngle = ccpToAngle(shootVector);
		CGFloat cocosAngle = CC_RADIANS_TO_DEGREES(-1 * shootAngle);
		
		float rotateSpeed = 0.5 / M_PI; // 1/2 second to roate 180 degrees
		float rotateDuration = fabs(shootAngle * rotateSpeed);    
		
		[self runAction:[CCSequence actions:
						 [CCRotateTo actionWithDuration:rotateDuration angle:cocosAngle],
						 [CCCallFunc actionWithTarget:self selector:@selector(finishFiring)],
						 nil]];		
	}
}

-(void)creepMoveFinished:(id)sender {
    
	DataModel * m = [DataModel getModel];
	
	CCSprite *sprite = (CCSprite *)sender;
	[self.parent removeChild:sprite cleanup:YES];
	
	[m._projectiles removeObject:sprite];
	
}

- (void)finishFiring {
	
	DataModel *m = [DataModel getModel];
	
	self.nextProjectile = [Projectile projectile];
	self.nextProjectile.position = self.position;
	
    [self.parent addChild:self.nextProjectile z:1];
    [m._projectiles addObject:self.nextProjectile];
    
	ccTime delta = 1.0;
	CGPoint shootVector = ccpSub(self.target.position, self.position);
	CGPoint normalizedShootVector = ccpNormalize(shootVector);
	CGPoint overshotVector = ccpMult(normalizedShootVector, 320);
	CGPoint offscreenPoint = ccpAdd(self.position, overshotVector);
	
	[self.nextProjectile runAction:[CCSequence actions:
								[CCMoveTo actionWithDuration:delta position:offscreenPoint],
								[CCCallFuncN actionWithTarget:self selector:@selector(creepMoveFinished:)],
								nil]];
	
	self.nextProjectile.tag = 2;		
	
    self.nextProjectile = nil;
    
}

@end
