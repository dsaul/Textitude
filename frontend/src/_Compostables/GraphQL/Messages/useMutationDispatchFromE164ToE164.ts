import { useMutation } from '@vue/apollo-composable'
import gql from 'graphql-tag';
import type { Ref } from 'vue';
import { provideApolloClient } from "@vue/apollo-composable";
import { useApolloClient } from '../useApolloClient';
import type { ApolloError } from '@apollo/client/errors';

const useMutationDispatchFromE164ToE164 = () => {
	
	const apolloClient = useApolloClient();
	provideApolloClient(apolloClient)
	
	const { mutate, onError, loading, onDone } = useMutation<{ status: string; }>(gql`
	mutation dispatchFromE164ToE164($fromE164: String!, $toE164: String!, $body: String!) {
		messages {
			dispatchFromE164ToE164(fromE164: $fromE164, toE164: $toE164, body: $body) {
				status
			}
		}
	}
	`);
	
	onError((error: ApolloError) => {
		console.error('useMutationDispatchFromE164ToE164 error', error);
	});
	
	const fn = (fromE164: Ref<string>, toE164: Ref<string>, body: Ref<string>) => {
		mutate({
			fromE164: fromE164.value,
			toE164: toE164.value,
			body: body.value,
		});
	};
	
	return {
		dispatch: fn,
		dispatchLoading: loading,
		dispatchError: onError,
		dispatchDone: onDone,
	}
}

export { useMutationDispatchFromE164ToE164 }