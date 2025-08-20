#!/bin/bash

# Test script for GitHub Actions workflow optimization
# This script validates the workflow YAML files and tests cost optimization logic

set -e

echo "🧪 Testing GitHub Actions Workflow Optimization"
echo "=============================================="

# Test 1: Validate YAML syntax
echo "📋 Test 1: Validating YAML syntax..."
python3 -c "
import yaml
import sys

files = [
    '.github/workflows/aws.yml',
    '.github/workflows/cancel-redundant-workflows.yml', 
    '.github/workflows/reusable-cancel-workflows.yml'
]

for file in files:
    try:
        with open(file, 'r') as f:
            yaml.safe_load(f)
        print(f'✅ {file} - Valid YAML')
    except yaml.YAMLError as e:
        print(f'❌ {file} - Invalid YAML: {e}')
        sys.exit(1)
    except Exception as e:
        print(f'❌ {file} - Error: {e}')
        sys.exit(1)
"

# Test 2: Check for required workflow elements
echo ""
echo "🔍 Test 2: Checking workflow structure..."

# Check main workflow has cost optimization job
if grep -q "cancel-redundant-workflows" .github/workflows/aws.yml; then
    echo "✅ Main workflow includes cost optimization job"
else
    echo "❌ Main workflow missing cost optimization job"
    exit 1
fi

# Check conditional execution
if grep -q "github.event.pull_request.merged" .github/workflows/aws.yml; then
    echo "✅ Main workflow has conditional execution logic"
else
    echo "❌ Main workflow missing conditional execution logic"
    exit 1
fi

# Test 3: Check concurrency groups
echo ""
echo "⚙️  Test 3: Validating concurrency control..."

if grep -A5 "concurrency:" .github/workflows/aws.yml | grep -q "github.event.pull_request.base.ref"; then
    echo "✅ Enhanced concurrency control implemented"
else
    echo "❌ Enhanced concurrency control missing"
    exit 1
fi

# Test 4: Check cancellation workflow triggers
echo ""
echo "🎯 Test 4: Validating cancellation workflow triggers..."

if grep -A10 "on:" .github/workflows/cancel-redundant-workflows.yml | grep -q "releasecandidate/"; then
    echo "✅ Cancellation workflow targets releasecandidate branches"
else
    echo "❌ Cancellation workflow missing releasecandidate triggers"
    exit 1
fi

# Test 5: Check reusable workflow parameters
echo ""
echo "🔧 Test 5: Validating reusable workflow interface..."

if grep -A20 "workflow_call:" .github/workflows/reusable-cancel-workflows.yml | grep -q "target-branch:"; then
    echo "✅ Reusable cancellation workflow has proper parameters"
else
    echo "❌ Reusable cancellation workflow missing required parameters"
    exit 1
fi

# Test 6: Verify GitHub API usage
echo ""
echo "🌐 Test 6: Checking GitHub API integration..."

if grep -q "actions/github-script@v7" .github/workflows/cancel-redundant-workflows.yml; then
    echo "✅ Uses latest GitHub script action"
else
    echo "❌ GitHub script action version issue"
    exit 1
fi

# Test 7: Check safety mechanisms
echo ""
echo "🛡️  Test 7: Validating safety mechanisms..."

# Check for current run protection
if grep -q "run.id === context.runId" .github/workflows/cancel-redundant-workflows.yml; then
    echo "✅ Current run protection implemented"
else
    echo "❌ Missing current run protection"
    exit 1
fi

# Check for age-based filtering
if grep -q "runAge.*60000" .github/workflows/cancel-redundant-workflows.yml; then
    echo "✅ Age-based filtering implemented"
else
    echo "❌ Missing age-based filtering"
    exit 1
fi

echo ""
echo "✅ All tests passed! Workflow optimization is properly implemented."
echo ""
echo "💡 Summary of optimizations:"
echo "   - Enhanced concurrency control for better resource management"
echo "   - Automatic cancellation of redundant workflows on PR close/merge"
echo "   - Smart cancellation for releasecandidate branch integrations" 
echo "   - Conditional job execution to skip expensive operations"
echo "   - Safety mechanisms to prevent cancelling important runs"
echo ""
echo "💰 Expected cost benefits:"
echo "   - Reduced workflow execution minutes"
echo "   - Faster feedback cycles"  
echo "   - Cleaner workflow run history"
echo "   - Better resource utilization"