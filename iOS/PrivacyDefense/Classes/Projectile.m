//
//  Projectile.m
//  Cocos2D Build a Tower Defense Game
//
//  Created by iPhoneGameTutorials on 4/4/11.
//  Copyright 2011 iPhoneGameTutorial.com All rights reserved.
//

#import "Projectile.h"

@implementation Projectile

+ (id)projectile {
	
    Projectile *projectile = nil;
    if ((projectile = [[[super alloc] initWithFile:@"Projectile.png"] autorelease])) {
		
    }
	
    return projectile;
    
}

- (void) dealloc
{  
    [super dealloc];
}

@end
