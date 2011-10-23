//
//  Wave.h
//  Cocos2D Build a Tower Defense Game
//
//  Created by iPhoneGameTutorials on 4/4/11.
//  Copyright 2011 iPhoneGameTutorial.com All rights reserved.
//

#import "cocos2d.h"

#import "Creep.h"

@interface Wave : CCNode {
	float _spawnRate;
	int _totalCreeps;
	Creep * _creepType;
}

@property (nonatomic) float spawnRate;
@property (nonatomic) int totalCreeps;
@property (nonatomic, retain) Creep *creepType;

- (id)initWithCreep:(Creep *)creep SpawnRate:(float)spawnrate TotalCreeps:(int)totalcreeps;

@end
