//
//  Creep.m
//  Cocos2D Build a Tower Defense Game
//
//  Created by iPhoneGameTutorials on 4/4/11.
//  Copyright 2011 iPhoneGameTutorial.com All rights reserved.
//

#import "Creep.h"

@implementation Creep

@synthesize hp = _curHp;
@synthesize moveDuration = _moveDuration;

@synthesize curWaypoint = _curWaypoint;

- (id) copyWithZone:(NSZone *)zone {
	Creep *copy = [[[self class] allocWithZone:zone] initWithCreep:self];
	return copy;
}

- (Creep *) initWithCreep:(Creep *) copyFrom {
  
    //code is not hitting here
    NSString *enemyImage;
    //enemyImage= @"Enemy1.png";
    enemyImage = @"Judge.png";
    
    
    if ((self = [[[super alloc] initWithFile:enemyImage] autorelease])) {
	self.hp = copyFrom.hp;
	self.moveDuration = copyFrom.moveDuration;
	self.curWaypoint = copyFrom.curWaypoint;
	}
	[self retain];
	return self;
}

- (WayPoint *)getCurrentWaypoint{
	
	DataModel *m = [DataModel getModel];
	
	WayPoint *waypoint = (WayPoint *) [m._waypoints objectAtIndex:self.curWaypoint];
	
	return waypoint;
}

- (WayPoint *)getNextWaypoint{
	
	DataModel *m = [DataModel getModel];
	int lastWaypoint = m._waypoints.count;

	self.curWaypoint++;
	
	if (self.curWaypoint > lastWaypoint)
		self.curWaypoint = lastWaypoint - 1;
	
	WayPoint *waypoint = (WayPoint *) [m._waypoints objectAtIndex:self.curWaypoint];
	
	return waypoint;
}

-(void)creepLogic:(ccTime)dt {
	
	
	// Rotate creep to face next waypoint
	WayPoint *waypoint = [self getCurrentWaypoint ];
	
	CGPoint waypointVector = ccpSub(waypoint.position, self.position);
	CGFloat waypointAngle = ccpToAngle(waypointVector);
	CGFloat cocosAngle = CC_RADIANS_TO_DEGREES(-1 * waypointAngle);
	
	float rotateSpeed = 0.5 / M_PI; // 1/2 second to roate 180 degrees
	float rotateDuration = fabs(waypointAngle * rotateSpeed);    
	
	[self runAction:[CCSequence actions:
					 [CCRotateTo actionWithDuration:rotateDuration angle:cocosAngle],
					 nil]];		
}

@end

@implementation FastRedCreep

+ (id)creep {
 
    FastRedCreep *creep = nil;

    //Specify the image
    NSString *enemyImage;
    //enemyImage= @"Enemy1.png";
    enemyImage = @"Corporate Creep.png";
    

    if ((creep = [[[super alloc] initWithFile:enemyImage] autorelease])) {
        creep.hp = 10;
        creep.moveDuration = 4;
		creep.curWaypoint = 0;
    }
	
	[creep schedule:@selector(creepLogic:) interval:0.2];
	
    return creep;
}

@end

@implementation StrongGreenCreep

+ (id)creep {
    
    StrongGreenCreep *creep = nil;
    
    //Specify the image
    NSString *enemyImage;
    //enemyImage= @"Enemy2.png";
    enemyImage = @"Data Miner Creep.png";
    

    if ((creep = [[[super alloc] initWithFile:enemyImage] autorelease])) {
        creep.hp = 20;
        creep.moveDuration = 9;
		creep.curWaypoint = 0;
    }
	
	[creep schedule:@selector(creepLogic:) interval:0.2];
    
	return creep;
}

@end

//ACLU Creeps

@implementation DataMinerCreep

+ (id)creep {
    
    DataMinerCreep *creep = nil;
    
    //Specify the image
    NSString *enemyImage;
    enemyImage = @"Data Miner Creep.png";
    
    
    if ((creep = [[[super alloc] initWithFile:enemyImage] autorelease])) {
        creep.hp = 20;
        creep.moveDuration = 7;
		creep.curWaypoint = 0;
    }
	
	[creep schedule:@selector(creepLogic:) interval:0.2];
    
	return creep;
}

@end

@implementation DefconZepplinCreep

+ (id)creep {
    
    DefconZepplinCreep *creep = nil;
    
    //Specify the image
    NSString *enemyImage;
    enemyImage = @"Defcon Zeppelin.png";
    
    
    if ((creep = [[[super alloc] initWithFile:enemyImage] autorelease])) {
        creep.hp = 20;
        creep.moveDuration = 20;
		creep.curWaypoint = 0;
    }
	
	[creep schedule:@selector(creepLogic:) interval:0.2];
    
	return creep;
}

@end

@implementation CorporateCreep

+ (id)creep {
    
    CorporateCreep *creep = nil;
    
    //Specify the image
    NSString *enemyImage;
    enemyImage = @"Corporate Creep.png";
    
    
    if ((creep = [[[super alloc] initWithFile:enemyImage] autorelease])) {
        creep.hp = 20;
        creep.moveDuration = 9;
		creep.curWaypoint = 0;
    }
	
	[creep schedule:@selector(creepLogic:) interval:0.2];
    
	return creep;
}

@end

@implementation GovSearcherCreep

+ (id)creep {
    
    StrongGreenCreep *creep = nil;
    
    //Specify the image
    NSString *enemyImage;
    //enemyImage= @"Enemy2.png";
    enemyImage = @"Gov Searcher Heli.png";
    
    
    if ((creep = [[[super alloc] initWithFile:enemyImage] autorelease])) {
        creep.hp = 20;
        creep.moveDuration = 5;
		creep.curWaypoint = 0;
    }
	
	[creep schedule:@selector(creepLogic:) interval:0.2];
    
	return creep;
}

@end