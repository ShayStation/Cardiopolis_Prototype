
#import <CoreMotion/CoreMotion.h>
#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

static CMPedometer *pedometer = nil;

#ifdef __cplusplus
extern "C" {
#endif

void StartStepTracking()
{
    if (![CMPedometer isStepCountingAvailable]) {
        NSLog(@"Step tracking not available.");
        return;
    }

    pedometer = [[CMPedometer alloc] init];

    NSCalendar *calendar = [NSCalendar currentCalendar];
    NSDate *now = [NSDate date];
    NSDate *startOfDay;
    [calendar rangeOfUnit:NSCalendarUnitDay startDate:&startOfDay interval:NULL forDate:now];

    [pedometer startPedometerUpdatesFromDate:startOfDay
                                 withHandler:^(CMPedometerData *data, NSError *error) {
        if (error) {
            NSLog(@"StartStepTracking error: %@", error.localizedDescription);
            return;
        }

        NSLog(@"StartStepTracking - steps: %@", data.numberOfSteps);
        NSString *stepString = [NSString stringWithFormat:@"%@", data.numberOfSteps];
        UnitySendMessage("StepTrackerManager", "OnStepUpdate", [stepString UTF8String]);
    }];
}

void StopStepTracking()
{
    if (pedometer != nil) {
        [pedometer stopPedometerUpdates];
        pedometer = nil;
        NSLog(@"Stopped step tracking.");
    }
}

void GetStepCountToday()
{
    NSLog(@"Manual step check triggered.");

    if (![CMPedometer isStepCountingAvailable]) {
        NSLog(@"Step counting not available.");
        UnitySendMessage("StepTrackerManager", "OnStepUpdate", "0");
        return;
    }

    CMPedometer *queryPedometer = [[CMPedometer alloc] init];
    NSCalendar *calendar = [NSCalendar currentCalendar];
    NSDate *now = [NSDate date];
    NSDate *startOfDay;
    [calendar rangeOfUnit:NSCalendarUnitDay startDate:&startOfDay interval:NULL forDate:now];

    [queryPedometer queryPedometerDataFromDate:startOfDay toDate:now withHandler:^(CMPedometerData *data, NSError *error) {
        if (error) {
            NSLog(@"GetStepCountToday error: %@", error.localizedDescription);
            UnitySendMessage("StepTrackerManager", "OnStepUpdate", "0");
            return;
        }

        NSLog(@"Queried step count: %@", data.numberOfSteps);
        NSString *stepString = [NSString stringWithFormat:@"%@", data.numberOfSteps];
        UnitySendMessage("StepTrackerManager", "OnStepUpdate", [stepString UTF8String]);
    }];
}

#ifdef __cplusplus
}
#endif
