// @flow
import * as React from 'react';
import {
    View,
    Text
} from 'react-native';

export function createGridRow(description: string, value: ?string): React.Node {
    return (
        <View style={{ flex: .2, flexDirection: 'row' }}>
            <View style={{ flex: 1 }}>
                <Text>{description}</Text>
            </View>
            <View style={{ flex: .8 }}>
                <Text>{value ? value : ""}</Text>
            </View>
        </View>
    );
}