// @flow 
import * as React from 'react';
import {
    StyleSheet
} from 'react-native';

const styles = StyleSheet.create({
    rootContainer:{
        flexDirection:'column',
        flex: 1,
        alignItems:'center', 
        justifyContent:'center'
    },
    container: {
        flex: 1,
        flexDirection:'row',
        justifyContent:'center',
        alignItems: 'center',
        backgroundColor: '#ddd',
        borderRadius:20
    },
    welcomeText:{
        fontSize: 16,
        flexWrap:'wrap',
        textAlign: 'center',
        fontWeight:'bold',
        paddingRight:2,
        paddingLeft:2
    },
    labelText:{
        fontWeight:'bold',
        paddingRight:2,
        paddingLeft:2
    },  
    textInput:{
        minWidth:200,
        borderWidth:2,
        borderColor:'#eee'
    },
    pickerButtonCancel:{
        fontSize: 18,
        color: '#007AFF',
        textAlign:'left'
    },
    pickerButtonDone: {
        fontSize: 18,
        color: '#007AFF',
        textAlign:'right'
    },
    pickerCancelButtonContainer:{
        flex:.5,
        alignItems:'flex-start'
    },
    pickerDoneButtonContainer: {
        flex:.5,
        alignItems:'flex-end'
    },
    pickerModalContent: {
        backgroundColor: 'white',
        padding: 22,
        borderRadius: 20,
        borderColor: 'rgba(0, 0, 0, 0.1)',
        flex: .33
    },
    pickerBottomModal: {
        flex: 1,
        justifyContent: 'flex-end'
    },
    pickerTouchableContainer: {
        flex: 1,
        flexDirection: 'row',
        justifyContent: 'center',
        alignItems: 'center',
        minHeight: 46,
        borderWidth: 1,
        borderColor: '#404041',
        paddingLeft: 6,
        paddingRight: 4,
        paddingTop: 10,
        marginBottom: 10,
    },

});

export function flattenedStylesheet(styleSheetProp:Object){
    return StyleSheet.flatten(styleSheetProp);
}

export default styles;

